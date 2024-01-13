using UnityEngine;

namespace SpaceShooter
{
    public class PowerUpStats : PowerUp
    {
        public enum EffectType
        {
            AddAmmo,
            AddEnergy
        }

        [SerializeField] private EffectType m_EffectType;

        [SerializeField] private float m_Value;

        protected override void OnPickedUp(SpaceShip ship)
        {
            if (m_EffectType == EffectType.AddEnergy)
            {
                ship.AddEnergy((int) m_Value);
                Debug.Log("AddEnergy");
            }
                


            if (m_EffectType == EffectType.AddAmmo)
            {
                ship.AddAmmo((int) m_Value);
                Debug.Log("AddAmmo");
            }
        }
    }
}

