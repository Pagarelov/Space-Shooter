using System.Collections;
using UnityEngine;


namespace SpaceShooter
{
    public class Player : SingeltonBase<Player>
    {
        [SerializeField] private int m_NumLives;
        [SerializeField] private SpaceShip m_Ship;
        [SerializeField] private GameObject m_PlayerShipPrefab;

        public SpaceShip ActiveShip => m_Ship;

        [SerializeField] private CameraController m_CameraController;
        [SerializeField] private MovementController m_MovementController;

        [SerializeField] private float m_Seconds;

        private void Start ()
        {
            m_Ship.EventOnDeath.AddListener(OnShipDeath);
        }

        private void OnShipDeath() 
        {
            m_NumLives--;

            if (m_NumLives > 0) 
                StartCoroutine(Respawn());
        } 

        private IEnumerator Respawn()
        {
            yield return new WaitForSeconds(m_Seconds);

            var newPlayerShip = Instantiate(m_PlayerShipPrefab);

            m_Ship = newPlayerShip.GetComponent<SpaceShip>();

            m_CameraController.SetTarget(m_Ship.transform);
            m_MovementController.SetTargetShip(m_Ship);

            m_Ship.EventOnDeath.AddListener(OnShipDeath);
        }
    }
}

