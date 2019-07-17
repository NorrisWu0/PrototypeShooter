using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Enemy Setting
    [Header("Enemy Setting", order = 0)]
    [Space(5, order = 1)]
    [SerializeField] private float m_Health;
    [SerializeField] private GameObject m_DeathFX;
    private float m_DeathFXTimer;
    private Rigidbody2D m_RB2D;

    [Space(20, order = 2)]
    #endregion

    #region Movement Setting
    [Header("Movement Setting", order = 3)]
    [Space(5, order = 4)]
    [SerializeField] private float m_MoveSpeed, m_RotateRate;

    private GameObject m_Target;
    #endregion

    private void Start()
    {
        m_RB2D = GetComponent<Rigidbody2D>();
        m_Target = GameObject.FindGameObjectWithTag("Player").gameObject;
        m_DeathFXTimer = m_DeathFX.GetComponent<ParticleSystem>().main.duration;
    }

    private void FixedUpdate()
    {
        ChaseTarget();
    }

    #region Enemy Function
    public void TakeDamage(float _damage)
    {
        m_Health -= _damage;

        if (m_Health < 0)
            Die();
    }

    private void Die()
    {
        GameObject _deathFX = Instantiate(m_DeathFX, transform.position, transform.rotation);
        Destroy(_deathFX, m_DeathFXTimer);

        // Slowly decrease alpha value of sprite

        Destroy(gameObject);
    }
    #endregion

    #region Movement Function
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

}
