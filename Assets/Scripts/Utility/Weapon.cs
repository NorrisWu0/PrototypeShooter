using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GeoShot
{
    [CreateAssetMenu(fileName = "Weapon_", menuName = "Weapon")]
    public class Weapon : ScriptableObject
    {
        [SerializeField] bool m_IsFiring = false;

        [Header("Weapon Stats")]
        [SerializeField] GameObject m_ProjectileType = null;
        [Range(60, 1200)]
        [SerializeField] float m_RPM = 0;
        [Range(0, 20)]
        [SerializeField] float m_Dispersion = 0;

        [Header("Weapon Config")]
        [SerializeField] AudioClip m_WeaponSFX = null;

        [SerializeField] float m_NextShotTimer = 0;

        public void FireWeapon(Transform _muzzle)
        {
            if (Time.time > m_NextShotTimer)
            {
                GameObject _projectile = PoolManager.Instance.RequestAvailableObject(m_ProjectileType.name, "ProjectilePools");
                _projectile.transform.position = _muzzle.position;
                _projectile.transform.rotation = _muzzle.rotation;
                _projectile.transform.Rotate(new Vector3(0, 0, Random.Range(-m_Dispersion, m_Dispersion)));
                _projectile.SetActive(true);

                AudioManager.Instance.weaponAudioSource.PlayOneShot(m_WeaponSFX);

                m_NextShotTimer = Time.time + (60 / m_RPM);
            }
        }


        int counter = 0;

        public void UpgradeWeapon()
        {
            counter++;
            m_RPM *= 1.2f;
            m_Dispersion *= 1.15f;
            Debug.Log(counter);
        }
    }
}
