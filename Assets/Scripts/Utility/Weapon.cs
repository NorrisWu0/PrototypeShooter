using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GeoShot
{
    [CreateAssetMenu(fileName = "Weapon_", menuName = "Weapon")]
    public class Weapon : ScriptableObject
    {
        public string weaponName;

        [Header("Weapon Stats")]
        public GameObject projectileType;
        [Range(60, 1200)]
        public float roundPerMinutes;
        [Range(1, 4)]
        public float secondsBeforeOverHeat;
        [Range(0, 20)]
        public float weaponDispersion;

        [Header("Weapon Config")]
        public AudioClip weaponSFX;
        public bool isFiring;
        public float heatLevel;

        [Space(20)]
        private float m_NextShotTimer;

        private void OnEnable()
        {
            //Debug.Log("My turn");
        }

        public void FireWeapon(Transform _muzzle)
        {
            if (Time.time > m_NextShotTimer && heatLevel < secondsBeforeOverHeat)
            {
                GameObject _projectile = PoolManager.Instance.RequestAvailableObject(projectileType.name, "ProjectilePools");
                _projectile.transform.position = _muzzle.position;
                _projectile.transform.rotation = _muzzle.rotation;
                _projectile.transform.Rotate(new Vector3(0, 0, Random.Range(-weaponDispersion, weaponDispersion)));
                _projectile.SetActive(true);

                //heatLevel += 1 / (roundPerMinutes / 60);
                //UIManager.Instance.UpdateHeatBar(heatLevel / secondsBeforeOverHeat);

                //AudioManager.Instance.weaponAudioSource.PlayOneShot(weaponSFX);

                m_NextShotTimer = Time.time + (60 / roundPerMinutes);
            }
        }

        IEnumerator CR_WeaponCooldown()
        {
            while (LevelManager.Instance.isPlaying && !isFiring)
            {
                if (heatLevel >= 0)
                    heatLevel -= Time.deltaTime / secondsBeforeOverHeat;

                yield return null;
            }
        }

    }

}
