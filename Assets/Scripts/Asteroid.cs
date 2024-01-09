using SpaceShooter;
using UnityEngine;
using UnityEngine.Events;



//[RequireComponent(typeof(StoneMovement))]
namespace SpaceShooter
{
    public class Asteroid : Destructible
    {
        public enum AsteroidSize
        {
            Small,
            Normal,
            Big,
            Huge
        }

        [SerializeField] private AsteroidSize size;
        [SerializeField] private float spawnUpForce;
        [SerializeField] private float spawnChance;

        public static UnityEvent<Asteroid> OnDestroyed = new UnityEvent<Asteroid>();
        public AsteroidSize Size => size;

        public void Awake()
        {
            EventOnDeath.AddListener(OnAsteroidDestroyed);

            SetSize(size);
        }

        private void OnDestroy()
        {
            EventOnDeath.RemoveListener(OnAsteroidDestroyed);
        }

        private void OnAsteroidDestroyed()
        {
            if (size != AsteroidSize.Small)
            {
                SpawnAsteroid();
            }
            OnDestroyed.Invoke(this);
            Destroy(gameObject);
        }

        private void SpawnAsteroid()
        {
            float zOffset = 0.01f;

            for (int i = 0; i < 2; i++)
            {
                Vector3 spawnPosition = transform.position + new Vector3(0.2f, 0.2f, zOffset * i);
                Asteroid asteroid = Instantiate(this, spawnPosition, Quaternion.identity);
                asteroid.SetSize(size - 1);
            }
        }

        public void SetSize(AsteroidSize size)
        {
            if (size < 0) return;

            transform.localScale = GetVectorFromSize(size);
            this.size = size;
        }

        public Vector3 GetVectorFromSize(AsteroidSize size)
        {
            if (size == AsteroidSize.Huge) return new Vector3(1, 1, 1);
            if (size == AsteroidSize.Big) return new Vector3(0.75f, 0.75f, 0.75f);
            if (size == AsteroidSize.Normal) return new Vector3(0.6f, 0.6f, 0.6f);
            if (size == AsteroidSize.Small) return new Vector3(0.4f, 0.4f, 0.4f);

            return Vector3.one;
        }
    }

}
