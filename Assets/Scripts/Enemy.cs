using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Enemy Setting
    [Header("Enemy Setting")]
    [SerializeField] float m_Health;
    [SerializeField] float m_MaxHealth;
    
    Rigidbody2D m_RB2D;
    #endregion

    #region VFX Setting
    [Header("VFX Setting")]
    public GameObject deathFX;

    #endregion

    #region Movement Setting
    [Header("Movement Setting")]
    [SerializeField] float m_MoveSpeed;
    [SerializeField] float m_RotateRate;

    [SerializeField] GameObject m_Target;
    #endregion

    #region Testing Area
    [SerializeField] Vector3 monitor_speed;
    [SerializeField] float monitor_alpha;
    #endregion

    private void Awake()
    {
        m_MaxHealth = m_Health;
        m_RB2D = GetComponent<Rigidbody2D>();
        m_Target = GameObject.FindGameObjectWithTag("Player").gameObject;
    }

    private void OnEnable()
    {
        m_Health = m_MaxHealth;
    }

    private void FixedUpdate()
    {
        if (m_Target.activeSelf)
            ChaseTarget();
    }

    #region TakeDamage
    public void TakeDamage(float _damage)
    {
        m_Health -= _damage;

        if (m_Health <= 0)
            Die();
    }
    #endregion

    #region Die
    private void Die()
    {
        deathFX.transform.parent = null;
        deathFX.SetActive(true);
        Invoke("DisableExplosionFX", 2f);

        gameObject.SetActive(false);
    }
    #endregion

    #region DisableExplosionFX
    void DisableExplosionFX()
    {
        deathFX.transform.parent = transform;
        deathFX.transform.localPosition = Vector3.zero;
        deathFX.SetActive(false);
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
