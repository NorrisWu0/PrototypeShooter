using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    [Header("Weapon System")]
    [Range(0,1)]
    [SerializeField] float m_FireRate;
    float m_FireRateTimer;

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
        if (Input.GetMouseButton(0))
            FireWeapon();
    }

    void FireWeapon()
    {
        if (Time.time > m_FireRateTimer)
        {
            m_ProjectilePoolIndex = NextAvaliableProjectile();
            m_ProjectilePool[m_ProjectilePoolIndex].transform.position = m_Muzzle.position;
            m_ProjectilePool[m_ProjectilePoolIndex].transform.rotation = m_Muzzle.rotation;
            m_ProjectilePool[m_ProjectilePoolIndex].gameObject.SetActive(true);

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
