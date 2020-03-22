using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace GeoShot
{
public class WeaponSystem : MonoBehaviour
{
    [Header("Weapon System")]
    [SerializeField] bool m_AutoFire;
    [SerializeField] bool m_IsRailgun;
    [SerializeField] bool m_IsShotgun;
    [Range(0, 2)]
    [SerializeField] float m_FireRate;
    [Range(0, 45)]
    [SerializeField] float m_DisperseRate;
    [SerializeField] int m_ShotgunPelletsCount;
    [SerializeField] AudioClip m_WeaponSFX;
    float m_FireRateTimer;

    [Header("Weaponry")]
    [SerializeField] int m_WeaponryIndex;
    [SerializeField] Weapon[] m_Weaponry;

    [Header("Weapon Setup")]
    [SerializeField] TextMeshProUGUI m_WeaponNameText;
    [SerializeField] Transform m_Muzzle;
    [SerializeField] Transform m_Pool;
    [SerializeField] int m_PoolIndex;
    [SerializeField] List<Projectile> m_ProjectilePool;
    [SerializeField] List<Projectile> m_RailgunPool;
    [SerializeField] GameObject m_ProjectilePrefab;

    private void Start()
    {
        SetupWeapon();
    }

    void Update()
    {
        if (Time.timeScale != 0)
        {
            if (Input.mouseScrollDelta.y > 0)
                SwitchWeapon(-1);
            else if (Input.mouseScrollDelta.y < 0)
                SwitchWeapon(1);

        
            if (m_AutoFire)
                InvokeRepeating("FireWeapon", 0, m_FireRate);
            else if (Input.GetMouseButton(0))
                FireWeapon();
        }
    }

    void SwitchWeapon(int _value)
    {
        m_WeaponryIndex = Mathf.Abs(m_WeaponryIndex + _value) % m_Weaponry.Length;
        SetupWeapon();
    }

    void SetupWeapon()
    {
        #region Fetch Weapon Value
        m_FireRate = m_Weaponry[m_WeaponryIndex].fireRate;
        m_DisperseRate = m_Weaponry[m_WeaponryIndex].disperseRate;
        m_WeaponSFX = m_Weaponry[m_WeaponryIndex].weaponSFX;
        m_ProjectilePrefab = m_Weaponry[m_WeaponryIndex].projectilePrefab;
        #endregion

        #region Check if is shotgun
        if (m_Weaponry[m_WeaponryIndex].isShotgun)
        {
            m_IsShotgun = m_Weaponry[m_WeaponryIndex].isShotgun;
            m_ShotgunPelletsCount = m_Weaponry[m_WeaponryIndex].shotgunPelletsCount;
        }
        else
            m_IsShotgun = false;
        #endregion

        #region Check if is railgun
        if (m_Weaponry[m_WeaponryIndex].isRailgun)
            m_IsRailgun = true;
        else
            m_IsRailgun = false;
        #endregion

        UpdateWeaponUI();
    }
    
    void UpdateWeaponUI()
    {
        m_WeaponNameText.SetText(m_Weaponry[m_WeaponryIndex].name);
    }

    void FireWeapon()
    {
        if (Time.time > m_FireRateTimer)
        {
            if (m_IsShotgun)
            {
                for (int i = 0; i < m_ShotgunPelletsCount; i++)
                {
                    m_PoolIndex = NextAvaliableProjectile(m_ProjectilePool);
                    m_ProjectilePool[m_PoolIndex].transform.position = m_Muzzle.position;
                    m_ProjectilePool[m_PoolIndex].transform.rotation = m_Muzzle.rotation;
                    m_ProjectilePool[m_PoolIndex].transform.Rotate(new Vector3(0, 0, Random.Range(-m_DisperseRate, m_DisperseRate)));
                    m_ProjectilePool[m_PoolIndex].gameObject.SetActive(true);

                }
            }
            else if (m_IsRailgun)
            {
                m_PoolIndex = NextAvaliableProjectile(m_RailgunPool);
                m_RailgunPool[m_PoolIndex].transform.position = m_Muzzle.position;
                m_RailgunPool[m_PoolIndex].transform.rotation = m_Muzzle.rotation;
                m_RailgunPool[m_PoolIndex].gameObject.SetActive(true);
            }
            else
            {
                m_PoolIndex = NextAvaliableProjectile(m_ProjectilePool);
                m_ProjectilePool[m_PoolIndex].transform.position = m_Muzzle.position;
                m_ProjectilePool[m_PoolIndex].transform.rotation = m_Muzzle.rotation;

                #region Disperse Projectile
                m_ProjectilePool[m_PoolIndex].transform.Rotate(new Vector3(0, 0, Random.Range(-m_DisperseRate, m_DisperseRate)));
                #endregion

                m_ProjectilePool[m_PoolIndex].gameObject.SetActive(true);
            }

            AudioManager.instance.weaponAudioSource.PlayOneShot(m_WeaponSFX);
            m_FireRateTimer = Time.time + m_FireRate;
        }
    }

    int NextAvaliableProjectile(List<Projectile> _pool)
    {
        for (int i = 0; i < _pool.Count; i++)
            if (!_pool[i].gameObject.activeSelf)
                return i;

        GameObject _projectileClone = Instantiate(m_ProjectilePrefab, m_Muzzle.position, m_Muzzle.rotation);
        _projectileClone.transform.parent = m_Pool;
        _pool.Add(_projectileClone.GetComponent<Projectile>());
        return _pool.Count - 1;
    }

}
}
