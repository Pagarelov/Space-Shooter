using UnityEngine;

namespace SpaceShooter
{
    /// <summary>
    /// Position limiter. Works in conjunction with the LevelBoundary script if there is one on the stage.
    /// Rushes at the object that needs to be limit.
    /// </summary>
    public class LevelBoundaryLimit : MonoBehaviour
    {
        private void Update()
        {
            if (LevelBoundary.Instance == null) return;

            var lb = LevelBoundary.Instance;
            var r = lb.Radius;

            if (transform.position.magnitude > r)
            {
                if (lb.LimitMode == LevelBoundary.Mode.Limit)
                {
                    transform.position = transform.position.normalized * r;
                }

                if (lb.LimitMode == LevelBoundary.Mode.Teleport)
                {
                    transform.position = -transform.position.normalized * r;
                }
            }
        }
    }
}

