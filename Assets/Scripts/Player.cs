using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    #region GameObject Variables Settings

    #region Player Setting
    [Header("Player Setting")]
    [SerializeField] float m_Health = 100;
    [SerializeField] float m_MaxHealth = 100;
    [SerializeField] int m_LiveCount = 3;
    [SerializeField] GameObject m_DeathFX;

    [SerializeField] HealthUIVariables m_HealthUIVariables = new HealthUIVariables();
    [System.Serializable]
    struct HealthUIVariables
    {
        public GameObject m_HealthTextUI;
        public GameObject m_HealthBarUI;
    }

    float m_DeathFXTimer = 1f;
    Rigidbody2D m_RB2D;
    #endregion

    #region Movement Setting
    [Header("Movement Setting")]
    [SerializeField] float m_PlayerSpeed = 60f;
    float m_MoveHorizontal;
    float m_MoveVertical;
    #endregion

    #region Weapon Setting
    [Header("Weapon Setting")]
    [SerializeField] int m_CurrentWeaponIndex = 0;
    [SerializeField] GameObject m_Projectile;
    [SerializeField] List<Bullet> m_Bullets;
    [SerializeField] Transform m_WeaponMuzzle;

    [SerializeField] WeaponUIVariables m_WeaponUIVariables = new WeaponUIVariables();
    [System.Serializable]
    struct WeaponUIVariables
    {
        public GameObject m_WeaponReloadUI;
        public GameObject m_AmmoUIText;
        public GameObject m_AmmoUIBar;
        public GameObject m_WeaponNameUI;
    }
    #endregion
    
    #region Audio Setting - TODO
    #endregion


    #endregion

    void Start()
    {
        m_RB2D = GetComponent<Rigidbody2D>();
        m_Health = m_MaxHealth;
        

        UpdateHealthInfo();
    }

    private void Update()
    {


    }

    void FixedUpdate()
    {
        MovePlayer();
        FollowMouse();
    }

    #region Player Functions
    public void TakeDamage(float _damage)
    {
        Debug.Log("Player took " + _damage + " damage");
        m_Health -= _damage;

        if (m_Health <= 100)
            KillPlayer();

        UpdateHealthInfo();
    }
    
    private void KillPlayer()
    {
        GameObject _DeathFXClone = Instantiate(m_DeathFX, transform.position, transform.rotation);
        Destroy(_DeathFXClone, m_DeathFXTimer);
    }
    #endregion

    #region Weapon Functions
    
    #endregion

    #region Movement Functions

    private void MovePlayer()
    {
        #region Get movement input
        m_MoveHorizontal = Input.GetAxis("Horizontal");
        m_MoveVertical = Input.GetAxis("Vertical");
        #endregion

        m_RB2D.velocity = new Vector2(m_MoveHorizontal, m_MoveVertical) * m_PlayerSpeed;

    }
    
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

    #region UI Functions
    private void UpdateHealthInfo()
    {
        m_HealthUIVariables.m_HealthTextUI.GetComponent<TextMeshProUGUI>().SetText("Health: " + m_Health + "/" + m_MaxHealth);
        m_HealthUIVariables.m_HealthBarUI.GetComponent<Slider>().value = m_Health / m_MaxHealth;
    }
    #endregion
}
