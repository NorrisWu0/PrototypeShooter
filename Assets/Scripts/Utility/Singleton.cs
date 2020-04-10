using UnityEngine;

namespace GeoShot
{
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;
        public static T Instance { get { return instance; } }

        [SerializeField] protected bool m_DontDestroyOnLoad = true;

        #region MonoBehaviour Lifecycle
        protected virtual void Awake()
        {
            InitSingleton(m_DontDestroyOnLoad);
        }

        protected virtual void OnApplicationQuit()
        {
            instance = null;
        }
        #endregion

        private void InitSingleton(bool _dontDestroyOnLoad = true)
        {
            // Ensure only one instance is kept (the first that was born)
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this.GetComponent<T>();

            if (_dontDestroyOnLoad && transform.parent == null)
                DontDestroyOnLoad(gameObject);
        }
    }
}