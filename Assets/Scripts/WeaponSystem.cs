using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    [Header("Weapon System")]
    [SerializeField] bool m_AutoFire;
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
    [SerializeField] Transform m_Muzzle;
    [SerializeField] Transform m_Pool;
    [SerializeField] int m_ProjectilePoolIndex;
    [SerializeField] List<Projectile> m_ProjectilePool;
    [SerializeField] GameObject m_ProjectilePrefab;

    [SerializeField] WeaponUIVariables m_WeaponUIVariables = new WeaponUIVariables();
    [System.Serializable]
    struct WeaponUIVariables
    {
        public GameObject m_WeaponReloadUI;
        public GameObject m_AmmoUIText;
        public GameObject m_AmmoUIBar;
        public GameObject m_WeaponNameUI;
    }
    
    void Update()
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

    void SwitchWeapon(int _value)
    {
        m_WeaponryIndex = Mathf.Abs(m_WeaponryIndex + _value) % m_Weaponry.Length;

        #region Fetch Weapon Value
        m_FireRate = m_Weaponry[m_WeaponryIndex].fireRate;
        m_DisperseRate = m_Weaponry[m_WeaponryIndex].disperseRate;
        m_WeaponSFX = m_Weaponry[m_WeaponryIndex].weaponSFX;

        if (m_Weaponry[m_WeaponryIndex].isShotgun)
        {
            m_IsShotgun = m_Weaponry[m_WeaponryIndex].isShotgun;
            m_ShotgunPelletsCount = m_Weaponry[m_WeaponryIndex].shotgunPelletsCount;
        }
        else
            m_IsShotgun = false;

        #endregion
    }

    void FireWeapon()
    {
        if (Time.time > m_FireRateTimer)
        {
            if (m_IsShotgun)
            {
                for (int i = 0; i < m_ShotgunPelletsCount; i++)
                {
                    m_ProjectilePoolIndex = NextAvaliableProjectile();
                    m_ProjectilePool[m_ProjectilePoolIndex].transform.position = m_Muzzle.position;
                    m_ProjectilePool[m_ProjectilePoolIndex].transform.rotation = m_Muzzle.rotation;
                    m_ProjectilePool[m_ProjectilePoolIndex].transform.Rotate(new Vector3(0, 0, Random.Range(-m_DisperseRate, m_DisperseRate)));
                    m_ProjectilePool[m_ProjectilePoolIndex].gameObject.SetActive(true);

                }

                AudioManager.instance.weaponAudioSource.PlayOneShot(m_WeaponSFX);
            }
            else
            {
                m_ProjectilePoolIndex = NextAvaliableProjectile();
                m_ProjectilePool[m_ProjectilePoolIndex].transform.position = m_Muzzle.position;
                m_ProjectilePool[m_ProjectilePoolIndex].transform.rotation = m_Muzzle.rotation;

                #region Disperse Projectile
                m_ProjectilePool[m_ProjectilePoolIndex].transform.Rotate(new Vector3(0, 0, Random.Range(-m_DisperseRate, m_DisperseRate)));
                #endregion

                m_ProjectilePool[m_ProjectilePoolIndex].gameObject.SetActive(true);
            
                AudioManager.instance.weaponAudioSource.PlayOneShot(m_WeaponSFX);
            }
            m_FireRateTimer = Time.time + m_FireRate;
        }
    }

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

}
