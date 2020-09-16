﻿using System.Collections;
using UnityEditor;
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

        /// <summary>
        /// Update reward when enemy is killed by the player
        /// </summary>
        public override void TakeDamage(float _damage)
        {
            base.TakeDamage(_damage);

            if (m_Health <= 0)
                LevelManager.Instance.UpdateScore(m_Reward);
        }
    }
}