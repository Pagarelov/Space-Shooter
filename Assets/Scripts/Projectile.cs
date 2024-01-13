using UnityEngine;

namespace SpaceShooter
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float m_Velocity;

        [SerializeField] private float m_Lifetime;

        [SerializeField] private int m_Damage;

        [SerializeField] private ImpactEffect m_InpactEffectPrefab;

        [SerializeField] private float rotationSpeed;

        private Destructible m_Parent;

        private float m_Timer;


        private void Update()
        {
            float stepLength = Time.deltaTime * m_Velocity;
            Vector2 step = transform.up * stepLength;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, stepLength);

            if (hit)
            {
                Destructible dest = hit.collider.transform.root.GetComponent<Destructible>();

                if (dest != null && dest != m_Parent)
                {
                    dest.ApplyDamage(m_Damage);
                }

                OnProjectileLifeEnd(hit.collider, hit.point);
            }

            m_Timer += Time.deltaTime;

            if (m_Timer > m_Lifetime)
                Destroy(gameObject);

            if (hit.collider == null)
            {
                if (rotationSpeed != 0)
                    RotateTowardsTarget(step);
            }

            transform.position += new Vector3(step.x, step.y, 0);
        }

        private void RotateTowardsTarget(Vector2 step)
        {
            Vector3 direction = step.normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

        private void OnProjectileLifeEnd(Collider2D col, Vector2 pos)
        {
            Destroy(gameObject);
        }

        public void SetParentShooter(Destructible parent)
        {
            m_Parent = parent;
        }
    }
}

