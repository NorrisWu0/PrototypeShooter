using UnityEngine;

namespace GeoShot
{ 
    public class Unit : MonoBehaviour
    {
        [SerializeField] protected float m_MaxHealth;
        protected float m_Health;
        protected bool m_IsAlive;

        void OnEnable()
        {
            m_Health = m_MaxHealth;
            m_IsAlive = true;
        }

        #region Take Damage
        public virtual void TakeDamage(float _damage)
        {
            m_Health -= _damage;

            if (m_Health <= 0)
                Die();
        }
        #endregion

        #region Die
        protected virtual void Die()
        {
            m_IsAlive = false;
            gameObject.SetActive(false);
        }
        #endregion
    }
}
