using UnityEngine;

namespace PrototypeShooter
{
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T m_Instance;
        public static T Instance { get { return m_Instance; } }

        [SerializeField] protected bool m_DontDestroyOnLoad = true;

        #region MonoBehaviour Lifecycle
        protected virtual void Awake()
        {
            InitSingleton(m_DontDestroyOnLoad);
        }

        protected virtual void OnApplicationQuit()
        {
            m_Instance = null;
        }
        #endregion

        private void InitSingleton(bool _dontDestroyOnLoad = true)
        {
            // Ensure only one instance is kept (the first that was born)
            if (m_Instance != null && m_Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            m_Instance = this.GetComponent<T>();

            if (_dontDestroyOnLoad && transform.parent == null)
                DontDestroyOnLoad(gameObject);
        }
    }
}