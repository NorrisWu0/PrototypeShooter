using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace PrototypeShooter
{
    public class Player : Unit
    {
        Rigidbody2D m_RB2D;

        protected override void Start()
        {
            base.Start();

            m_IsAlive = true;
            m_RB2D = GetComponent<Rigidbody2D>();

            InitWeapon();
        }

        private void Update()
        {
            if (Input.GetMouseButton(0) && LevelManager.Instance.isPlaying)
                m_Weapon.FireWeapon(m_Muzzle);
        }

        void FixedUpdate()
        {
            MovePlayer();
        }

        #region Movement System
        [Header("Movement")]
        [SerializeField] float m_PlayerSpeed = 60f;

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
        #endregion

        #region Weapon System
        [Header("Weapon System")]
        [SerializeField] Transform m_Muzzle = null;
        [SerializeField] Weapon m_Weapon = null;

        /// <summary>
        /// Create a copy of each (SO)Weapon to break off the reference to original weapon asset.
        /// </summary>
        private void InitWeapon()
        {
            m_Weapon = Instantiate(m_Weapon);
        }
        
        /// <summary>
        /// Powerup weapon.
        /// </summary>
        public void WeaponPowerup()
        {
            m_Weapon.UpgradeWeapon();
        }
        #endregion

        #region Vitality System
        /// <summary>
        /// Regenerate 3 - 10% of health.
        /// </summary>
        public void RegenHealth()
        {
            m_Health += (m_MaxHealth * Random.Range(0.03f, 0.1f));

            if (m_Health > m_MaxHealth)
                m_Health = m_MaxHealth;

            UIManager.Instance.UpdateHealthBar(m_Health, m_MaxHealth);
        }

        /// <summary>
        /// Take damage when hit, and ask UIManager to update health UI.
        /// </summary>
        public override void TakeDamage(float _damage)
        {
            base.TakeDamage(_damage);
            UIManager.Instance.UpdateHealthBar(m_Health, m_MaxHealth);
        }

        /// <summary>
        /// Kill the player and shake camera when destroyed
        /// </summary>
        protected override void Die()
        {
            m_CameraController.enableLerpToMidPoint = false;
            m_CameraController.StartCameraZoon(10);
            m_CameraController.StartCameraShake(0.3f, 3.0f);

            LevelManager.Instance.EndLevel();

            base.Die();
        }
        #endregion
    }
}