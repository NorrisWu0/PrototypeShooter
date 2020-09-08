﻿using System.Collections;
using UnityEngine;

namespace GeoShot
{
    public class SpawnManager : MonoBehaviour
    {
        #region Struct - SpawnType
        [System.Serializable]
        class SpawnType
        {
            public GameObject entity = null;
            [Range(0,1f)]
            public float spawnChance = 0;
        }
        #endregion

        [SerializeField] SpawnType[] m_SpawnList = null;
        [SerializeField] float m_SpawnDelay = 0;
        [SerializeField] float m_SpawnRadius = 0;

        private Transform m_Target = null;

        private void Start()
        {
            // Search player in the scene
            m_Target = GameObject.FindGameObjectWithTag("Player").transform;

            if (m_Target != null)
                StartCoroutine(CR_SpawnEntity());
            else
                Debug.LogError("SpawneSystem: m_Target couldn't not be found");

        }

        /// <summary>
        /// Spawn entity based on the setting in spawn list;
        /// </summary>
        IEnumerator CR_SpawnEntity()
        {
            while (LevelManager.Instance.isPlaying)
            {
                foreach (SpawnType _spawnItem in m_SpawnList)
                    if (Random.value < _spawnItem.spawnChance)
                        if (m_Target != null)
                        {
                            // Fetch entity from Pool Manager
                            GameObject _entity = PoolManager.Instance.RequestAvailableObject(_spawnItem.entity.name, "EnemyPools");
                            // Get random position at defined radius.
                            Vector2 _spawnPos = m_Target.position;
                            _spawnPos += Random.insideUnitCircle.normalized * m_SpawnRadius;

                            // Move and activate entity
                            if (_entity != null)
                            {
                                _entity.transform.position = _spawnPos;
                                _entity.SetActive(true);
                            }
                        }

                yield return new WaitForSeconds(m_SpawnDelay);
            }

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