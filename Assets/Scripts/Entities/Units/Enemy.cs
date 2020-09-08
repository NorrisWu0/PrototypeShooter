using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace GeoShot
{
    public class Enemy : Unit
    {
        [Header("Movement Config")]
        [SerializeField] float m_Velocity = 0;
        [SerializeField] float m_RotateSpeed = 0;
        
        [Header("Enemy Config")]
        [SerializeField] int m_Reward = 0;
        
        private Rigidbody2D m_RB2D;
        private GameObject m_Target = null;

        private void Awake()
        {
            m_RB2D = GetComponent<Rigidbody2D>();
            if (GameObject.FindGameObjectWithTag("Player").gameObject != null)
                m_Target = GameObject.FindGameObjectWithTag("Player").gameObject;
        }

        private void Update()
        {
            if (m_Target != null && m_Target.activeSelf)
                ChaseTarget();
        }

        /// <summary>
        /// Rotate and move enemy towards target.
        /// </summary>
        private void ChaseTarget()
        {
            // Rotate enemy towards target - Stole from Sugar Warrior
            Vector2 _direction = (m_Target.transform.position - transform.position).normalized;
            float _RotateAngleangle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg - 90f;
            m_RB2D.rotation = Mathf.LerpAngle(m_RB2D.rotation, _RotateAngleangle, m_RotateSpeed);

            // Move enemy forward
            m_RB2D.velocity = transform.up.normalized * m_Velocity;
        }

        /// <summary>
        /// Return value of reward for killing this enemy.
        /// </summary>
        /// <returns></returns>
        public int GetReward()
        {
            return m_Reward;
        }


        /// <summary>
        /// Damage player when colliding with them
        /// </summary>
        /// <param name="_collision"></param>
        private void OnCollisionEnter2D(Collision2D _collision)
        {
            if (_collision.gameObject.CompareTag("Player"))
            {
                _collision.gameObject.GetComponent<Player>().TakeDamage(m_Health);
                Die();
            }
        }
    }
}