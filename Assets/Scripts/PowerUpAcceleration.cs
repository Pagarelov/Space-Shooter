using UnityEngine;

namespace SpaceShooter
{
    public class PowerUpAcceleration : PowerUp
    {
        [SerializeField] private float m_AccelerationMultiplier = 2f;
        [SerializeField] private float m_AccelerationDuration = 10f;
        protected override void OnPickedUp(SpaceShip ship)
        {
            Debug.Log("PowerUpAcceleration");
            ship.ActivateAcceleration(m_AccelerationMultiplier, m_AccelerationDuration);
        }
    }
}

