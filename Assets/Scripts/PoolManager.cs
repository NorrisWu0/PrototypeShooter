﻿using System.Collections.Generic;
using UnityEngine;

namespace GeoShot
{
    public class PoolManager : Singleton<PoolManager>
    {
        #region Struct - MasterPool, Pool
        [System.Serializable]
        struct MasterPool
        {
            public string masterTag;
            public int objectsPerPool;
            public SubPool[] subPools;
        }

        [System.Serializable]
        struct SubPool
        {
            [HideInInspector]
            public string poolTag;
            public GameObject prefab;
            public List<GameObject> pool;
        }
        #endregion

        [SerializeField] MasterPool[] m_MasterPools;

        private void Start()
        {
            foreach (MasterPool _masterPool in m_MasterPools)
                InitializePools(_masterPool);
        }

        /// <summary>
        /// Preload the pool with objects.
        /// </summary>
        private void InitializePools(MasterPool _masterPool)
        {
            // Create & rename a new GameObject to hold sub pools under masterpool.
            GameObject _emptyMasterPool = new GameObject();
            _emptyMasterPool.name = _masterPool.masterTag;
            _emptyMasterPool.transform.parent = transform;

            // Initialize sub pool tag based on the prefab assigned to them
            for (int i = 0; i < _masterPool.subPools.Length; i++)
            {
                if (_masterPool.subPools[i].prefab != null)
                    _masterPool.subPools[i].poolTag = _masterPool.subPools[i].prefab.name;
                else
                    _masterPool.subPools[i].poolTag = "This prefab is not assigned!";
            }

            // Initializing sub pools
            for (int i = 0; i < _masterPool.subPools.Length; i++)
            {
                // Create & rename a new GameObject to hold items in sub pool
                GameObject _newObject = new GameObject();
                _newObject.transform.parent = _emptyMasterPool.transform;
                _newObject.name = _masterPool.subPools[i].prefab.name;

                // Instantiate a number of objects into the sub pool.
                for (int j = 0; j < _masterPool.objectsPerPool + 1; j++)
                {
                    // Check if this pool has a prefab assigned.
                    if (_masterPool.subPools[i].prefab != null)
                    {
                        GameObject _clone = Instantiate(_masterPool.subPools[i].prefab, _newObject.transform);
                        _clone.SetActive(false);
                        _masterPool.subPools[i].pool.Add(_clone);
                    }
                    else
                    {
                        Debug.LogError("Pool " + _masterPool.subPools[i].poolTag + " doesn't have a prefab in it!!!");
                        break;
                    }
                }

            }
        }

        /// <summary>
        /// Request a object from the matched pool (_poolTag) in master pool (_masterPoolTag).
        /// </summary>
        public GameObject RequestAvailableObject(string _poolTag, string _masterTag)
        {
            foreach (MasterPool _masterPool in m_MasterPools)
                if (_masterPool.masterTag == _masterTag)
                    foreach (SubPool _pool in _masterPool.subPools)
                    {
                        if (_pool.poolTag == _poolTag)
                        {
                            for (int i = 0; i < _pool.pool.Count; i++)
                            {
                                if (!_pool.pool[i].gameObject.activeSelf)
                                    return _pool.pool[i];
                            }

                            GameObject _clone = Instantiate(_pool.prefab, transform);
                            _clone.SetActive(false);
                            _pool.pool.Add(_clone);
                            return _pool.pool[_pool.pool.Count - 1];
                        }
                    }
        
            Debug.LogError("This pool you trying to access doesn't exist! Check the tag you using! (" + _poolTag + ").");
            return null;
        }
    }
}