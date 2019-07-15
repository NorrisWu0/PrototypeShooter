using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject m_EnemyPrefeb;
    [SerializeField] private bool m_IsSpawing;
    [SerializeField] private float m_SpawnRadius;
    [Tooltip("How many second to spawn an enemy.")]
    [SerializeField] private float m_SpawnRate;
    [SerializeField] private float m_SpawnRateTimer;

    private Vector2 m_SpawnPos;

    private void Update()
    {
        if (m_IsSpawing && Time.time > m_SpawnRateTimer)
        {
            m_SpawnPos = GameObject.FindGameObjectWithTag("Player").transform.position;
            m_SpawnPos += Random.insideUnitCircle.normalized * m_SpawnRadius;

            Instantiate(m_EnemyPrefeb, m_SpawnPos, Quaternion.identity);
            m_SpawnRateTimer = Time.time + m_SpawnRate;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(GameObject.FindGameObjectWithTag("Player").transform.position, m_SpawnRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(m_SpawnPos, 1f);

    }
}
