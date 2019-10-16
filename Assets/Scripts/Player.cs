using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    #region Player Setting
    [Header("Player Setting")]
    public bool isAlive;
    [SerializeField] float m_Health;
    public float maxHealth;

    [SerializeField] float m_PlayerSpeed = 60f;
    float m_MoveHorizontal;
    float m_MoveVertical;

    [Header("Player Setup")]
    [SerializeField] GameObject m_DeathVFX;
    [SerializeField] AudioClip m_DeathSFX;
    [SerializeField] HealthUIVariables m_HealthUIVariables = new HealthUIVariables();
    [System.Serializable]
    struct HealthUIVariables
    {
        public TextMeshProUGUI m_HealthText;
        public Slider m_HealthBar;
        public float m_HealthBarLerpRate;
    }

    float m_DeathFXTimer = 1f;
    Rigidbody2D m_RB2D;
    #endregion
    
    void Start()
    {
        isAlive = true;
        m_Health = maxHealth;
        m_RB2D = GetComponent<Rigidbody2D>();
        StartCoroutine("UpdateHealthUI");
    }
    
    void FixedUpdate()
    {
        MovePlayer();
        FollowMouse();
    }

    #region Player Functions
    #region Take Damage
    public void TakeDamage(float _damage)
    {
        m_Health -= _damage;

        if (m_Health <= 0)
            Die();

        if (gameObject.activeSelf)
            StartCoroutine("UpdateHealthUI");
    }
    #endregion

    #region Die
    private void Die()
    {
        isAlive = false;

        GameObject _DeathFXClone = Instantiate(m_DeathVFX, transform.position, transform.rotation);
        Destroy(_DeathFXClone, m_DeathFXTimer);

        EnemySpawner.m_IsSpawing = false;
        AudioManager.instance.playerAudioSource.PlayOneShot(m_DeathSFX);
        gameObject.SetActive(false);
    }
    #endregion
    
    #region Move Player - Mobile and Input Axis
    private void MovePlayer()
    {
        Vector2 _movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        m_RB2D.velocity = _movement * m_PlayerSpeed;

    }
    #endregion
    
    #region Follow Mouse
    private void FollowMouse()
    {
        #region Get mouse direction in world space
        Vector3 _MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 _MouseDirection = new Vector2(_MousePosition.x - transform.position.x, _MousePosition.y - transform.position.y);
        #endregion

        #region Rotate player towards mouse position
        transform.up = _MouseDirection;
        #endregion
    }
    #endregion

    #region Update Health UI
    IEnumerator UpdateHealthUI()
    {
        while (m_HealthUIVariables.m_HealthBar.value != m_Health / maxHealth && gameObject.activeSelf)
        {
            m_HealthUIVariables.m_HealthText.SetText("Health: " + (m_Health / maxHealth).ToString("00%"));
            m_HealthUIVariables.m_HealthBar.value = Mathf.Lerp(m_HealthUIVariables.m_HealthBar.value, (m_Health / maxHealth), m_HealthUIVariables.m_HealthBarLerpRate);

            yield return null;
        }
    }
    #endregion
    #endregion
}
