using UnityEngine;

namespace SpaceShooter
{
    public class RandomTransferFinish : MonoBehaviour
    {
        [SerializeField] private GameObject finishPrefab;
        [SerializeField] private LevelBoundary levelBoundary;

        private static bool finishSpawned = false;

        private void Start()
        {
            if (!finishSpawned)
            {
                SpawnFinishInRandomPosition();
                finishSpawned = true;
            }
        }

        private void SpawnFinishInRandomPosition()
        {
            Vector3 randomPosition = GetRandomPositionWithinBoundary();

            if (CheckCollision(randomPosition))
            {
                Debug.LogWarning("Spawn position is colliding with other objects. Trying again...");
                SpawnFinishInRandomPosition();
                return;
            }

            Instantiate(finishPrefab, randomPosition, Quaternion.identity);
        }

        private Vector3 GetRandomPositionWithinBoundary()
        {
            float randomAngle = Random.Range(0f, 360f);
            Vector3 randomDirection = Quaternion.Euler(0, randomAngle, 0) * Vector3.forward;
            Vector3 randomPosition = levelBoundary.transform.position + randomDirection * Random.Range(0f, levelBoundary.Radius);

            return randomPosition;
        }

        private bool CheckCollision(Vector3 position)
        {
            Collider[] colliders = Physics.OverlapSphere(position, 1f);

            foreach (var collider in colliders)
            {
                if (collider.gameObject != gameObject)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
