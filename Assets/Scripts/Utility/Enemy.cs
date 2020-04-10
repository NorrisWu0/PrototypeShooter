using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace GeoShot
{
    public class Enemy : Unit
    {
        #region Enemy Setting
        [Header("Enemy Setting")]
        [SerializeField] float m_MoveSpeed = 0;
        [SerializeField] float m_RotateRate = 0;
        [SerializeField] float m_Reward = 0;
        [SerializeField] GameObject m_Target = null;
        #endregion

        #region Enemy Setup
        [Header("Enemy Setup")]
        public GameObject deathVFX;
        [SerializeField] AudioClip m_DeathSFX;
        Rigidbody2D m_RB2D;
        #endregion

        private void Awake()
        {
            m_MaxHealth = m_Health;
            m_RB2D = GetComponent<Rigidbody2D>();
            m_Target = GameObject.FindGameObjectWithTag("Player").gameObject;
        }

        private void FixedUpdate()
        {
            if (m_Target.activeSelf)
                ChaseTarget();
        }

        #region TakeDamage
        public override void TakeDamage(float _damage)
        {
            m_Health -= _damage;

            if (m_Health <= 0)
            {
                Die();
            }
        }
        #endregion

        #region Die
        protected override void Die()
        {
            deathVFX.transform.parent = null;
            deathVFX.SetActive(true);
            Invoke("DisableExplosionFX", 2f);

            AudioManager.instance.enemyAudioSource.PlayOneShot(m_DeathSFX);

            gameObject.SetActive(false);
        }
        #endregion

        #region DisableExplosionFX
        void DisableExplosionFX()
        {
            deathVFX.transform.parent = transform;
            deathVFX.transform.localPosition = Vector3.zero;
            deathVFX.SetActive(false);
        }
        #endregion

        #region ChaseTarget
        private void ChaseTarget()
        {
            #region Rotate enemy towards target
            // Stole from Sugar Warrior
            Vector2 _direction = (m_Target.transform.position - transform.position).normalized;
            float _RotateAngleangle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg - 90f;
            m_RB2D.rotation = Mathf.LerpAngle(m_RB2D.rotation, _RotateAngleangle, m_RotateRate);
            #endregion

            #region Move enemy towards Target
            m_RB2D.velocity = transform.up.normalized * m_MoveSpeed;
            #endregion
        }
        #endregion
    
        #region OnCollisionEnter2D
        private void OnCollisionEnter2D (Collision2D _collision)
        {
            if (_collision.gameObject.CompareTag("Player"))
            {
                _collision.gameObject.GetComponent<Player>().TakeDamage(m_Health);
                Die();
            }
        }
        #endregion
    }
}