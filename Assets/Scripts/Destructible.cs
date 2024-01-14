using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SpaceShooter
{
    /// <summary>
    /// Destroyed object on stage. Something that can have hit points.
    /// </summary>
    public class Destructible : Entity
    {
        #region Properties

        /// <summary>
        /// Player ignores damage.
        /// </summary>
        [SerializeField] protected bool m_Indestructible;
        public bool IsIndestructible => m_Indestructible;

        /// <summary>
        /// Starting number of hit points.
        /// </summary>
        [SerializeField] private int m_HitPoints;

        /// <summary>
        /// Current hit points.
        /// </summary>
        private int m_CurrentHitPoints;
        public int HitPoints => m_CurrentHitPoints;

        [SerializeField] private UnityEvent m_EventOnDeath;
        public UnityEvent EventOnDeath => m_EventOnDeath;

        [SerializeField] private GameObject particleEffectPrefab;


        private static HashSet<Destructible> m_AllDestructibles = null;

        public static IReadOnlyCollection<Destructible> AllDestructibles => m_AllDestructibles;


        public const int TeamIdNeutral = 0;

        [SerializeField] private int m_TeamId;
        public int TeamId => m_TeamId;

        #endregion


        #region Unity Events

        protected virtual void Start()
        {
            m_CurrentHitPoints = m_HitPoints;
            m_Indestructible = false;
        }

        #endregion


        #region Public API
        /// <summary>
        /// Applying damage to an object.
        /// </summary>
        /// <param name="damage">Damage done to an object</param>
        public void ApplyDamage(int damage)
        {
            if (m_Indestructible) return;

            m_CurrentHitPoints -= damage;

            if (m_CurrentHitPoints <= 0)
                OnDeath();
        }

        #endregion

        /// <summary>
        /// Overrideable object destruction event when hitpoints are below zero.
        /// </summary>

        protected virtual void OnDeath()
        {
            SpawnParticleEffect();
            m_EventOnDeath?.Invoke();
            Destroy(gameObject);
        }

        private void SpawnParticleEffect()
        {
            if (particleEffectPrefab != null)
            {
                GameObject effect = Instantiate(particleEffectPrefab, transform.position, Quaternion.identity);
                Destroy(effect, 2f);
            }
        }

        #region Indestructible
        public void ToggleIndestructible(bool value)
        {
            m_Indestructible = value;
        }

        public void ActivateInvincibility(float duration)
        {
            if (!m_Indestructible)
            {
                m_Indestructible = true;

                StartCoroutine(DeactivateInvincibility(duration));
            }
        }

        private IEnumerator DeactivateInvincibility(float duration)
        {
            yield return new WaitForSeconds(duration);

            m_Indestructible = false;
        }
        #endregion

        protected virtual void OnEnable()
        {
            if (m_AllDestructibles == null)
                m_AllDestructibles = new HashSet<Destructible>();
            m_AllDestructibles.Add(this);
        }

        protected virtual void OnDestroy()
        {
            m_AllDestructibles.Remove(this);
        }

    }

}
