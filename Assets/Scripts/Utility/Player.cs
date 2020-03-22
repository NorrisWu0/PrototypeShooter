using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GeoShot
{
    public class Player : Unit
    {
        [Header("Movement")]
        [SerializeField] float m_PlayerSpeed = 60f;
        
        Rigidbody2D m_RB2D;

        void Start()
        {
            m_IsAlive = true;
            m_RB2D = GetComponent<Rigidbody2D>();
        
            StartCoroutine("UpdateHealthUI");
        }

        void FixedUpdate()
        {
            MovePlayer();
        }

        /// <summary>
        /// Rotate player toward mouse position and move player based on input from axis.
        /// </summary>
        private void MovePlayer()
        {
            // Get mouse direction in world space
            Vector3 _MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            // Calculate the direction to mouse
            Vector2 _MouseDirection = new Vector2(_MousePosition.x - transform.position.x, _MousePosition.y - transform.position.y);

            // Rotate player
            transform.up = _MouseDirection;

            // Get input from axis
            Vector2 _movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            // Move player
            m_RB2D.velocity = _movement * m_PlayerSpeed;
        }

        /// <summary>
        /// Take damage when hit, and ask UIManager to update health UI.
        /// </summary>
        /// <param name="_damage"></param>
        public override void TakeDamage(float _damage)
        {
            base.TakeDamage(_damage);
            UIManager.Instance.healthUI.UpdateHealthUI(m_Health, m_MaxHealth);
        }
    }
}