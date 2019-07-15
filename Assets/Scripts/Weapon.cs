using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[CreateAssetMenu(fileName = "Weapon_", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    #region Weapon Variables
    public string weaponName;
    [SerializeField] private GameObject m_Bullet;
    [SerializeField] private int m_BulletLifeTime;
    [SerializeField] private int m_CurrentAmmo;
    [SerializeField] private int m_MagCapacity, m_TotalAmmo;
    [SerializeField] private float m_ReloadRate, m_ReloadRateTimer, m_FireRate, m_FireRateTimer;
    [SerializeField] private bool m_IsReloading = false;
    [SerializeField] private float m_DisperseValue;

    // Need a better way to make this work
    private GameObject m_AmmoInfoUI;
    private TextMeshProUGUI m_WeaponReloadUIText, m_CurrentAmmoUIText, m_MagCapacityUIText, m_WeaponNameUIText;
    private Slider m_CurrantAmmoUIBar;
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

    #region Weapon UI Functions
    public void SetupWeaponVariables(GameObject _weaponReloadUI, GameObject _ammoInfoUI, GameObject _currentAmmoUI, GameObject _magCapacityUI, GameObject _currantAmmoBarUI, GameObject _weaponNameUI)
    {
        m_FireRateTimer = 0f;

        m_WeaponReloadUIText = _weaponReloadUI.GetComponent<TextMeshProUGUI>();
        m_CurrentAmmoUIText = _currentAmmoUI.GetComponent<TextMeshProUGUI>();
        m_MagCapacityUIText = _magCapacityUI.GetComponent<TextMeshProUGUI>();
        m_CurrantAmmoUIBar = _currantAmmoBarUI.GetComponent<Slider>();
        m_WeaponNameUIText = _weaponNameUI.GetComponent<TextMeshProUGUI>();

        // Need a better way to make this work
        m_AmmoInfoUI = _ammoInfoUI;

        // Fix if other weapon is reloading, weapon info won't display issue. (Because AmmoUI is disabled during last weapon reload function)
        if (m_IsReloading)
        {
            m_AmmoInfoUI.SetActive(false);
            m_WeaponReloadUIText.gameObject.SetActive(true);
        }
        else
        {
            m_AmmoInfoUI.SetActive(true);
            m_WeaponReloadUIText.gameObject.SetActive(false);
        }
        UpdateWeaponInformationUI();

    }

    private void UpdateWeaponInformationUI()
    {
        m_CurrentAmmoUIText.SetText(m_CurrentAmmo.ToString());
        m_MagCapacityUIText.SetText(m_TotalAmmo.ToString());
        m_CurrantAmmoUIBar.value = (m_CurrentAmmo * 1f / m_MagCapacity * 1f);
        m_WeaponNameUIText.SetText(weaponName.ToString());
    }

    private void ToggleReloadUI(bool _value)
    {
        m_WeaponReloadUIText.gameObject.SetActive(_value);
        // Need a better way to make this work
        m_AmmoInfoUI.SetActive(!_value);
    }
    #endregion
}
