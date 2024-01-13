using System.Collections;
using UnityEngine;

namespace SpaceShooter
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class SpaceShip : Destructible
    {
        /// <summary>
        /// Weight for automatic rigid installation.
        /// </summary>
        [Header("Space ship")]
        [SerializeField] private float m_Mass;

        /// <summary>
        /// Pushing force forward.
        /// </summary>
        [SerializeField] protected float m_Thrust;

        /// <summary>
        /// Rotating force.
        /// </summary>
        [SerializeField] protected float m_Mobility;

        /// <summary>
        /// Maximum linear speed.
        /// </summary>
        [SerializeField] protected float m_MaxLinearVelocity;

        /// <summary>
        /// Maximum rotational speed.In degrees/sec.
        /// </summary>
        [SerializeField] protected float m_MaxAngularVelocity;

        /// <summary>
        /// Saved link to rigid.
        /// </summary>
        private Rigidbody2D m_Rigid;

        private bool m_IsAccelerated;
        private float m_AccelerationMultiplier;
        private float m_AccelerationDuration;
        private float m_AccelerationTimer;

        private float m_OriginalThrust;

        #region Public API

        /// <summary>
        /// Linear traction control. -1.0 to +1.0.
        /// </summary>
        public float ThrustControl { get; set; }

        /// <summary>
        /// Rotational thrust control. -1.0 to +1.0.
        /// </summary>
        public float TorqueControl { get; set; }

        #endregion

        #region Unity Event

        protected override void Start()
        {
            base.Start();

            m_Rigid = GetComponent<Rigidbody2D>();
            m_Rigid.mass = m_Mass;

            m_Rigid.inertia = 1;

            m_IsAccelerated = false;
            m_AccelerationMultiplier = 1f;
            m_AccelerationDuration = 0f;
            m_AccelerationTimer = 0f;

            m_OriginalThrust = m_Thrust;

            InitOffensive();
        }

        private void FixedUpdate()
        {
            UpdateRigidbody();

            UpdateEnergyRegen();
        }

        #endregion

        /// <summary>
        /// Method of adding forces to a ship for movement.
        /// </summary>
        private void UpdateRigidbody()
        {
            m_Rigid.AddForce(ThrustControl * m_Thrust * transform.up * Time.fixedDeltaTime, ForceMode2D.Force);

            m_Rigid.AddForce(-m_Rigid.velocity * (m_Thrust / m_MaxLinearVelocity) * Time.fixedDeltaTime, ForceMode2D.Force);

            m_Rigid.AddTorque(TorqueControl * m_Mobility * Time.fixedDeltaTime, ForceMode2D.Force);

            m_Rigid.AddTorque(-m_Rigid.angularVelocity * (m_Mobility / m_MaxAngularVelocity) * Time.fixedDeltaTime, ForceMode2D.Force);

            if (m_IsAccelerated)
            {
                // Apply acceleration multiplier to thrust during acceleration
                m_Thrust = m_AccelerationMultiplier * m_OriginalThrust;
                m_Rigid.AddForce(m_Thrust * ThrustControl * transform.up * Time.fixedDeltaTime, ForceMode2D.Force);
            }
            else
            {
                // Apply normal thrust without acceleration
                m_Thrust = m_OriginalThrust;
                m_Rigid.AddForce(m_Thrust * ThrustControl * transform.up * Time.fixedDeltaTime, ForceMode2D.Force);
            }
        }

        [SerializeField] private Turret[] m_Turrets;

        public void Fire(TurretMode mode)
        {
            for (int i = 0; i < m_Turrets.Length; ++i)
            {
                if (m_Turrets[i].Mode == mode)
                {
                    m_Turrets[i].Fire();
                }
            }
        }

        [SerializeField] private int m_MaxEnergy;
        [SerializeField] private int m_MaxAmmo;
        [SerializeField] private int m_EnergyRegenPerSecond;

        private float m_PrimaryEnergy;
        private int m_SecondaryAmmo;

        public void AddEnergy(int energy)
        {
            m_PrimaryEnergy = Mathf.Clamp(m_PrimaryEnergy + energy, 0, m_MaxEnergy);
        }

        public void AddAmmo(int ammo)
        {
            m_SecondaryAmmo = Mathf.Clamp(m_SecondaryAmmo + ammo, 0, m_MaxAmmo);
        }
        
        private void InitOffensive()
        {
            m_PrimaryEnergy = m_MaxEnergy;
            m_SecondaryAmmo = m_MaxAmmo;
        }

        private void UpdateEnergyRegen()
        {
            m_PrimaryEnergy += (float)m_EnergyRegenPerSecond * Time.fixedDeltaTime;
            m_PrimaryEnergy = Mathf.Clamp(m_PrimaryEnergy, 0, m_MaxEnergy);
        }

        public bool DrawEnergy(int count)
        {
            if (count == 0)
                return true;

            if (m_PrimaryEnergy >= count)
            {
                m_PrimaryEnergy -= count;
                return true;
            }

            return false;
        }

        public bool DrawAmmo(int count)
        {
            if (count == 0)
                return true;

            if (m_SecondaryAmmo >= count)
            {
                m_SecondaryAmmo -= count;
                return true;
            }

            return false;
        }

        public void AssignWeapon(TurretProperties props)
        {
            for (int i = 0; i < m_Turrets.Length; i++)
            {
                m_Turrets[i].AssignLoadout(props);
            }
        }

        public void ActivateAcceleration(float multiplier, float duration)
        {
            m_AccelerationMultiplier = multiplier;
            m_AccelerationDuration = duration;
            m_AccelerationTimer = duration;
            m_IsAccelerated = true;

            // Start a coroutine to deactivate acceleration after a certain duration
            StartCoroutine(DeactivateAcceleration());
        }

        private IEnumerator DeactivateAcceleration()
        {
            while (m_AccelerationTimer > 0f)
            {
                Debug.Log(m_AccelerationTimer);
                yield return new WaitForSeconds(1f);
                m_AccelerationTimer--;
            }

            m_IsAccelerated = false;
            m_Thrust = m_OriginalThrust;
        }
    }
}

