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

        #region Ubity Event

        protected override void Start()
        {
            base.Start();

            m_Rigid = GetComponent<Rigidbody2D>();
            m_Rigid.mass = m_Mass;

            m_Rigid.inertia = 1;
        }

        private void FixedUpdate()
        {
            UpdateRigidbody();
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
    }
}

