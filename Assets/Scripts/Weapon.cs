using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[CreateAssetMenu(fileName = "Weapon_", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    #region Weapon Variables
    [Header("Weapon Setting")]
    public string weaponName;
    [SerializeField] private GameObject m_Bullet;
    [SerializeField] private int m_BulletLifeTime;
    [SerializeField] private int m_CurrentAmmo;
    [SerializeField] private int m_MagCapacity, m_TotalAmmo;
    [SerializeField] private float m_ReloadRate, m_ReloadRateTimer, m_FireRate, m_FireRateTimer;
    [SerializeField] private bool m_IsReloading = false;
    [SerializeField] private float m_DisperseValue;

    [Header("UI Setting")]
    // Need a better way to make this work
    private TextMeshProUGUI m_WeaponReloadUIText, m_AmmoUIText, m_WeaponNameUIText;
    private Slider m_AmmoUIBar;

    [Header("Audio Setting")]
    public AudioClip m_FiringAudio;
    public AudioClip m_ReloadAudio;
    public AudioClip m_BlankFireAudio;
    #endregion

    #region Weapon Function
    public void FireWeapon(Transform _muzzle)
    {
        if (m_CurrentAmmo > 0 && !m_IsReloading && Time.time > m_FireRateTimer)
        {
            GameObject _bulletClone = Instantiate(m_Bullet, _muzzle.position, _muzzle.rotation);
            if (m_DisperseValue != 0)
                _bulletClone.transform.Rotate(0, 0, Random.Range(-m_DisperseValue, m_DisperseValue));
            Destroy(_bulletClone, m_BulletLifeTime);
            m_CurrentAmmo--;
            m_FireRateTimer = Time.time + m_FireRate;

            #region PlayFiringAudio
            GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>().PlayOneShot(m_FiringAudio);
            #endregion
        }

        UpdateWeaponInformationUI();
    }

    public IEnumerator ReloadWeapon()
    {
        m_IsReloading = true;
        ToggleReloadUI(m_IsReloading);
        yield return new WaitForSeconds(m_ReloadRate);
        if (m_TotalAmmo > 0)
        {
            if (m_CurrentAmmo == 0)
            {
                if (m_TotalAmmo > m_MagCapacity)
                {
                    m_CurrentAmmo = m_MagCapacity;
                    m_TotalAmmo -= m_MagCapacity;
                }
                else
                {
                    m_CurrentAmmo = m_TotalAmmo;
                    m_TotalAmmo = 0;
                }
            }
            else
            {
                m_TotalAmmo -= (m_MagCapacity - m_CurrentAmmo);
                m_CurrentAmmo = m_MagCapacity;
            }
        }
        m_IsReloading = false;
        ToggleReloadUI(m_IsReloading);
        UpdateWeaponInformationUI();
    }

    private void DisperseProjectile()
    {

    }
    #endregion

    #region UI Functions
    public void SetupWeaponVariables(GameObject _weaponReloadUI, GameObject _ammoUIText, GameObject _ammoUIBar, GameObject _weaponUIName)
    {
        m_FireRateTimer = 0f;

        m_WeaponReloadUIText = _weaponReloadUI.GetComponent<TextMeshProUGUI>();
        m_AmmoUIText = _ammoUIText.GetComponent<TextMeshProUGUI>();
        m_AmmoUIBar = _ammoUIBar.GetComponent<Slider>();
        m_WeaponNameUIText = _weaponUIName.GetComponent<TextMeshProUGUI>();

        // Fix if other weapon is reloading, weapon info won't display issue. (Because AmmoUI is disabled during last weapon reload function)
        if (m_IsReloading)
            ToggleReloadUI(!m_IsReloading);
        else
            ToggleReloadUI(m_IsReloading);

        UpdateWeaponInformationUI();

    }

    private void UpdateWeaponInformationUI()
    {
        m_AmmoUIText.SetText(m_CurrentAmmo.ToString() + "/" + m_TotalAmmo.ToString());
        m_AmmoUIBar.value = (m_CurrentAmmo * 1f / m_MagCapacity);
        m_WeaponNameUIText.SetText(weaponName.ToString());
    }

    private void ToggleReloadUI(bool _value)
    {
        m_WeaponReloadUIText.gameObject.SetActive(_value);
        // Need a better way to make this work
        m_AmmoUIText.gameObject.SetActive(!_value);
    }
    #endregion
}
