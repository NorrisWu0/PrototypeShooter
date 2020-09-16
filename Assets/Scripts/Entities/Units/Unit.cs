using UnityEngine;

namespace GeoShot
{
    public class Unit : Entity
    {
        [Header("Basic Config")]
        [SerializeField] protected float m_MaxHealth;
        [SerializeField] GameObject m_DeathVFX;
        [SerializeField] AudioClip m_DeathSFX;
        
        protected float m_Health;
        protected bool m_IsAlive;

        void OnEnable()
        {
            m_Health = m_MaxHealth;
            m_IsAlive = true;
        }

        /// <summary>
        /// Take damage and kill the unit when health reaches 0.
        /// </summary>
        public virtual void TakeDamage(float _damage)
        {
            m_Health -= _damage;

            if (m_Health <= 0)
                Die();
        }

        /// <summary>
        /// This will kill this unit, play DeathVFX and DeathSFX.
        /// </summary>
        protected virtual void Die()
        {
            m_IsAlive = false;
            gameObject.SetActive(false);

            PlayDeathVFX();
            AudioManager.Instance.effectAudioSource.PlayOneShot(m_DeathSFX);
        }

        private void PlayDeathVFX()
        {
            GameObject _particleSystem = PoolManager.Instance.RequestAvailableObject(m_DeathVFX.name, "EffectPools");

            if (_particleSystem != null)
            {
                _particleSystem.SetActive(true);
                _particleSystem.transform.position = transform.position;
                _particleSystem.GetComponent<ParticleSystem>().Play();
            }
        }
    }
}
