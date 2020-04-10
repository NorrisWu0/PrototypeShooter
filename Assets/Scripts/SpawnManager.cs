using System.Collections;
using UnityEngine;

namespace GeoShot
{
    public class SpawnManager : MonoBehaviour
    {
        #region Struct - SpawnType
        [System.Serializable]
        class SpawnType
        {
            public Entity entity = null;
            [Range(0,1f)]
            public float spawnChance = 0;
        }
        #endregion

        [SerializeField] SpawnType[] m_SpawnList = null;
        [SerializeField] float m_SpawnRadius = 0;

        private Transform m_Target = null;

        private void Start()
        {
            // Search player in the scene
            m_Target = GameObject.FindGameObjectWithTag("Player").transform;
        }

        /// <summary>
        /// Spawn entity based on the setting in spawn list;
        /// </summary>
        IEnumerator CR_SpawnEntity()
        {
            while (LevelManager.Instance.isPlaying)
                foreach (SpawnType _spawnItem in m_SpawnList)
                    if (Random.value < _spawnItem.spawnChance)
                        if (m_Target != null)
                        {
                            // Fetch entity from Pool Manager
                            GameObject _entity = PoolManager.Instance.RequestAvailableObject(_spawnItem.entity.entityID, "EnemyPool");
                            // Get random position at defined radius.
                            Vector3 _spawnPos = m_Target.position * Random.insideUnitCircle * m_SpawnRadius;

                            // Move and activate entity
                            _entity.transform.position = _spawnPos;
                            _entity.SetActive(true);
                        }
                        else
                            Debug.LogError("SpawneSystem: m_Target couldn't not be found");

            yield return null;
        }

        #region Debug
        [Header("Debug Setting")]
        [SerializeField] Color m_DebugColor = Color.white;

        private void OnDrawGizmosSelected()
        {
            if (GameManager.Instance != null && GameManager.Instance.toggleDebugMode)
            {
                Gizmos.color = m_DebugColor;
                Gizmos.DrawWireSphere(m_Target.position, m_SpawnRadius);
            }
        }
        #endregion
    }
}
