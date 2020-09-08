using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GeoShot
{
    public class Player : Unit
    {
        Rigidbody2D m_RB2D;

        void Start()
        {
            m_IsAlive = true;
            m_RB2D = GetComponent<Rigidbody2D>();

            InitWeaponry();
        }

        void FixedUpdate()
        {
            MovePlayer();

            WeaponLoop();
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
        [SerializeField] Weapon[] m_Weaponry = null;

        private int m_WeaponryIndex = 0;

        /// <summary>
        /// Create a copy of each (SO)Weapon to break off the reference to original weapon asset.
        /// </summary>
        private void InitWeaponry()
        {
            Weapon[] _temp = new Weapon[m_Weaponry.Length];
            for (int i = 0; i < m_Weaponry.Length; i++)
                _temp[i] = Instantiate(m_Weaponry[i]);

            m_Weaponry = _temp;
        }

        /// <summary>
        /// The Weapon System's Update()
        /// </summary>
        private void WeaponLoop()
        {
            if (Time.timeScale != 0)
            {
                if (Input.mouseScrollDelta.y > 0)
                    SwitchWeapon(-1);
                else if (Input.mouseScrollDelta.y < 0)
                    SwitchWeapon(1);

                if (Input.GetMouseButton(0))
                {
                    FireWeapon();
                    m_Weaponry[m_WeaponryIndex].isFiring = true;
                }
                else
                    m_Weaponry[m_WeaponryIndex].isFiring = false;
            }
        }

        /// <summary>
        /// Switching the weapon by modifying the index value to determine which weapon to use in the weaponry.
        /// </summary>
        private void SwitchWeapon(int _value)
        {
            m_WeaponryIndex = Mathf.Abs(m_WeaponryIndex + _value) % m_Weaponry.Length;
            UIManager.Instance.SetWeaponName(m_Weaponry[m_WeaponryIndex].weaponName);
        }

        /// <summary>
        /// Fire current weapon.
        /// </summary>
        private void FireWeapon()
        {
            m_Weaponry[m_WeaponryIndex].FireWeapon(m_Muzzle);
        }
        #endregion

        #region Vitality System
        /// <summary>
        /// Take damage when hit, and ask UIManager to update health UI.
        /// </summary>
        public override void TakeDamage(float _damage)
        {
            base.TakeDamage(_damage);
            UIManager.Instance.UpdateHealthBar(m_Health, m_MaxHealth);
        }
        #endregion
    }
}