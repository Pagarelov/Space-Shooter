using UnityEngine;

namespace SpaceShooter
{
    /// <summary>
    /// The base class of all interactive game objects in the scene.
    /// </summary>
    public abstract class Entity : MonoBehaviour
    {
        /// <summary>
        /// Object name for user.
        /// </summary>
        [SerializeField] private string m_Nickname;
        public string NickName => m_Nickname;
    }
}

