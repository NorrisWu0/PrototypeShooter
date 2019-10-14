using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    #region Enemy Setting
    [Header("Enemy Setting")]
    [SerializeField] float m_Health;
    [SerializeField] float m_MaxHealth;
    
    [SerializeField] float m_MoveSpeed;
    [SerializeField] float m_RotateRate;

    [SerializeField] float m_Reward;
    [SerializeField] GameObject m_Target;
    #endregion

    #region Enemy Setup
    [Header("Enemy Setup")]
    [SerializeField] Slider m_HealthBar;
    [SerializeField] float m_HealthBarLerpRate;
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

        if (m_HealthBar != null)
            StartCoroutine("UpdateUI");

        if (m_Health <= 0)
        {
            GameManager.instance.AddScore(m_Reward);
            Die();
        }
    }
    #endregion

    IEnumerator UpdateUI()
    {
        while (m_HealthBar.value != m_Health / m_MaxHealth)
        {
            m_HealthBar.value = Mathf.Lerp(m_HealthBar.value, (m_Health / m_MaxHealth), m_HealthBarLerpRate);
            yield return null;
        }
    }

    #region Die
    private void Die()
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
