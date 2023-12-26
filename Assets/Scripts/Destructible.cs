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
        [SerializeField] private bool m_Indestructible;
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

        #endregion


        #region Unity Events

        protected virtual void Start()
        {
            m_CurrentHitPoints = m_HitPoints;
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
            Destroy(gameObject);
            m_EventOnDeath?.Invoke();
        }

        [SerializeField] private UnityEvent m_EventOnDeath;
        public UnityEvent EventOnDeath => m_EventOnDeath;
    }

}
