using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GeoShot
{
    public class Weapon : ScriptableObject
    {
        private bool m_IsFiring = false;

        [Header("Weapon Stats")]
        [SerializeField] GameObject m_ProjectileType = null;
        [Range(60, 1200)]
        [SerializeField] float m_RPM = 0;
        [Range(0, 20)]
        [SerializeField] float m_Dispersion = 0;

        [Header("Weapon Config")]
        [SerializeField] AudioClip m_WeaponSFX = null;
        [SerializeField] float m_NextShotTimer = 0;
        [SerializeField] float m_RPMModifier = 0;
        [SerializeField] float m_DispersionModifier = 0;

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

        /// <summary>
        /// Upgrade the weapon when player picks up a powerup
        /// </summary>
        public void UpgradeWeapon()
        {
            m_RPM *= m_RPMModifier;
            m_Dispersion *= m_DispersionModifier;
        }
    }
}
