using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    public PlayerData playerData { get; set; }

    #region Player Setting
    [Header("Player Setting")]
    [Range(20, 400)]
    [SerializeField] float m_Health;
    public float maxHealth;

    [SerializeField] float m_PlayerSpeed = 60f;
    float m_MoveHorizontal;
    float m_MoveVertical;

    [Header("Player Setup")]
    [SerializeField] LeftJoystick m_LeftJoystick;
    [SerializeField] RightJoystick m_RightJoystick;
    [SerializeField] Transform m_ReferencePoint;
    [SerializeField] GameObject m_DeathFX;
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
    
    private void OnEnable()
    {
        playerData = PlayerPersistence.LoadData();

        maxHealth = playerData.maxHealth;
        fireRate = playerData.fireRate;
        disperseRate = playerData.disperseValue;
    }

    void Start()
    {
        m_Health = maxHealth;
        m_RB2D = GetComponent<Rigidbody2D>();
        StartCoroutine("UpdateHealthUI");
    }
    
    void FixedUpdate()
    {
        MovePlayer();
        RotatePlayer();
        //FollowMouse();
    }

    private void Update()
    {
        if (m_RightJoystick.GetInputDirection() != Vector3.zero)
            FireWeapon();
    }

    private void OnDisable()
    {
        PlayerPersistence.SaveData(this);
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
        GameObject _DeathFXClone = Instantiate(m_DeathFX, transform.position, transform.rotation);
        Destroy(_DeathFXClone, m_DeathFXTimer);

        EnemySpawner.m_IsSpawing = false;
        gameObject.SetActive(false);
    }
    #endregion
    
    #region Move Player - Mobile and Input Axis
    private void MovePlayer()
    {
        Vector2 _movement;

        if (m_LeftJoystick != null)
            _movement = m_LeftJoystick.GetInputDirection();
        else
            _movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        m_RB2D.velocity = _movement * m_PlayerSpeed;

    }
    #endregion

    #region Rotate Player - Mobile Input
    void RotatePlayer()
    {
        Vector2 _direction = m_RightJoystick.GetInputDirection() * 5;
        
        if (_direction != Vector2.zero)
        {
            m_ReferencePoint.position = transform.position + new Vector3(_direction.x, _direction.y);
            transform.LookAt(m_ReferencePoint);
            transform.Rotate(new Vector3(0, 90, 90));
        }
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

    #region Simplified Weapon System
    [Header("Simplified Weapon System")]
    [Range(0.05f, 1.4f)]
    public float fireRate;
    [Range(1, 5)]
    public float disperseRate;
    [SerializeField] AudioClip m_WeaponSFX;
    float m_FireRateTimer;

    [Header("Weapon Setup")]
    [SerializeField] Transform m_Muzzle;
    [SerializeField] Transform m_Pool;
    [SerializeField] int m_ProjectilePoolIndex;
    [SerializeField] List<Projectile> m_ProjectilePool;
    [SerializeField] GameObject m_ProjectilePrefab;

    #region Fire Weapon
    void FireWeapon()
    {
        if (Time.time > m_FireRateTimer)
        {
            m_ProjectilePoolIndex = NextAvaliableProjectile();
            m_ProjectilePool[m_ProjectilePoolIndex].transform.position = m_Muzzle.position;
            m_ProjectilePool[m_ProjectilePoolIndex].transform.rotation = m_Muzzle.rotation;

            #region Disperse Projectile
            m_ProjectilePool[m_ProjectilePoolIndex].transform.Rotate(new Vector3(0, 0, Random.Range(-disperseRate, disperseRate)));
            #endregion

            m_ProjectilePool[m_ProjectilePoolIndex].gameObject.SetActive(true);

            AudioManager.instance.weaponAudioSource.PlayOneShot(m_WeaponSFX);

            m_FireRateTimer = Time.time + fireRate;
        }
    }
    #endregion

    #region Next Available Projectile
    int NextAvaliableProjectile()
    {
        for (int i = 0; i < m_ProjectilePool.Count; i++)
            if (!m_ProjectilePool[i].gameObject.activeSelf)
                return i;

        GameObject _projectileClone = Instantiate(m_ProjectilePrefab, m_Muzzle.position, m_Muzzle.rotation);
        _projectileClone.transform.parent = m_Pool;
        m_ProjectilePool.Add(_projectileClone.GetComponent<Projectile>());
        return m_ProjectilePool.Count - 1;
    }
    #endregion

    #endregion
}
