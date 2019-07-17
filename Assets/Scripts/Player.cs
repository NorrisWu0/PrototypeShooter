using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    #region GameObject Variables Settings

    #region Player Setting
    [Header("Player Setting", order = 0)]
    [Space(5, order = 1)]
    [SerializeField] float m_Health = 100;
    [SerializeField] float m_MaxHealth = 100;
    [SerializeField] int m_LiveCount = 3;
    [SerializeField] GameObject m_DeathFX;

    [SerializeField] HealthUIVariables m_HealthUIVariables = new HealthUIVariables();
    [System.Serializable]
    class HealthUIVariables
    {
        public GameObject m_HealthTextUI;
        public GameObject m_HealthBarUI;
    }

    float m_DeathFXTimer = 1f;
    Rigidbody2D m_RB2D;
    
    [Space(20, order = 2)]
    #endregion

    #region Movement Setting
    [Header("Movement Setting", order = 3)]
    [Space(5, order = 4)]
    [SerializeField] float m_PlayerSpeed = 60f;
    float m_MoveHorizontal;
    float m_MoveVertical;

    [Space(20, order = 5)]
    #endregion

    #region Weapon Setting
    [Header("Weapon Setting", order = 6)]
    [Space(5, order = 7)]
    [SerializeField] Weapon[] m_WeaponsOwned;
    [SerializeField] int m_CurrentWeaponIndex = 0;
    [SerializeField] Weapon m_CurrentWeapon;
    [SerializeField] Transform m_WeaponMuzzle;

    [SerializeField] WeaponUIVariables m_WeaponUIVariables = new WeaponUIVariables();
    [System.Serializable]
    class WeaponUIVariables
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

        m_CurrentWeapon = m_WeaponsOwned[m_CurrentWeaponIndex];
        // THIS IS UGLY!!!!!!!!!!!!!!!!!!!!!!
        m_CurrentWeapon.SetupWeaponVariables(m_WeaponUIVariables.m_WeaponReloadUI, 
            m_WeaponUIVariables.m_AmmoUIText, m_WeaponUIVariables.m_AmmoUIBar,
            m_WeaponUIVariables.m_WeaponNameUI);

        UpdateHealthInfo();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
            m_CurrentWeapon.FireWeapon(m_WeaponMuzzle);

        if (Input.GetKeyDown(KeyCode.R))
            StartCoroutine(m_CurrentWeapon.ReloadWeapon());

        if (Input.mouseScrollDelta.y > 0)
            LastWeapon();

        if (Input.mouseScrollDelta.y < 0)
            NextWeapon();
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
    public void AddWeapon(Weapon _newWeapon)
    {
    }

    private void NextWeapon()
    {
        m_CurrentWeaponIndex++;
        if (m_CurrentWeaponIndex < m_WeaponsOwned.Length)
        {
            m_CurrentWeapon = m_WeaponsOwned[m_CurrentWeaponIndex];
            // THIS IS UGLY!!!!!!!!!!!!!!!!!!!!!!
            m_CurrentWeapon.SetupWeaponVariables(m_WeaponUIVariables.m_WeaponReloadUI,
                m_WeaponUIVariables.m_AmmoUIText, m_WeaponUIVariables.m_AmmoUIBar,
                m_WeaponUIVariables.m_WeaponNameUI);
        }
        else
        {
            m_CurrentWeaponIndex = 0;
            m_CurrentWeapon = m_WeaponsOwned[m_CurrentWeaponIndex];
            // THIS IS UGLY!!!!!!!!!!!!!!!!!!!!!!
            m_CurrentWeapon.SetupWeaponVariables(m_WeaponUIVariables.m_WeaponReloadUI,
                m_WeaponUIVariables.m_AmmoUIText, m_WeaponUIVariables.m_AmmoUIBar,
                m_WeaponUIVariables.m_WeaponNameUI);
        }
    }

    private void LastWeapon()
    {
        m_CurrentWeaponIndex--;
        if (m_CurrentWeaponIndex < 0)
        {
            m_CurrentWeaponIndex = 2;
            m_CurrentWeapon = m_WeaponsOwned[m_CurrentWeaponIndex];
            // THIS IS UGLY!!!!!!!!!!!!!!!!!!!!!!
            m_CurrentWeapon.SetupWeaponVariables(m_WeaponUIVariables.m_WeaponReloadUI,
                m_WeaponUIVariables.m_AmmoUIText, m_WeaponUIVariables.m_AmmoUIBar,
                m_WeaponUIVariables.m_WeaponNameUI);
        }
        else
        {
            m_CurrentWeapon = m_WeaponsOwned[m_CurrentWeaponIndex];
            // THIS IS UGLY!!!!!!!!!!!!!!!!!!!!!!
            m_CurrentWeapon.SetupWeaponVariables(m_WeaponUIVariables.m_WeaponReloadUI,
                m_WeaponUIVariables.m_AmmoUIText, m_WeaponUIVariables.m_AmmoUIBar,
                m_WeaponUIVariables.m_WeaponNameUI);
        }
    }
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
