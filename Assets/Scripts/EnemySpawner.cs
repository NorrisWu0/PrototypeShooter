using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawner Setting")]
    [SerializeField] private bool m_IsSpawing;
    [SerializeField] private float m_SpawnRadius;
    [Range(0, 2)]
    [SerializeField] private float m_SpawnRate;

    private float m_SpawnRateTimer;
    private Vector2 m_SpawnPos;

    [Header("Spawner Setup")]
    [SerializeField] Transform m_Pool;
    [SerializeField] int m_EnemyIndex;
    [SerializeField] List<Enemy> m_Enemies;
    [SerializeField] private GameObject m_EnemyPrefeb;

    private void Update()
    {
        if (m_IsSpawing && Time.time > m_SpawnRateTimer)
        {
            m_SpawnPos = GameObject.FindGameObjectWithTag("Player").transform.position;
            m_SpawnPos += Random.insideUnitCircle.normalized * m_SpawnRadius;
            m_SpawnRateTimer = Time.time + m_SpawnRate;

            #region Pooling Enemy
            m_EnemyIndex = NextAvailableEnemy();
            m_Enemies[m_EnemyIndex].transform.position = m_SpawnPos;
            m_Enemies[m_EnemyIndex].gameObject.SetActive(true);
            #endregion

        }
    }

    int NextAvailableEnemy()
    {
        for (int i = 0; i < m_Enemies.Count; i++)
            if (!m_Enemies[i].gameObject.activeSelf && !m_Enemies[i].deathFX.activeSelf)
                return i;

        GameObject _clone = Instantiate(m_EnemyPrefeb, m_SpawnPos, Quaternion.identity);
        _clone.transform.parent = m_Pool;
        m_Enemies.Add(_clone.GetComponent<Enemy>());
        return m_Enemies.Count - 1;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(GameObject.FindGameObjectWithTag("Player").transform.position, m_SpawnRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(m_SpawnPos, 1f);

    }
}
