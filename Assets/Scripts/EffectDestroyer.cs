using UnityEngine;

namespace SpaceShooter
{
    public class EffectDestroyer : MonoBehaviour
    {
        [SerializeField] private float destroyDelay = 2f;

        private void Start()
        {
            Destroy(gameObject, destroyDelay);
        }
    }
}

