using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace GeoShot
{
    public class UIManager : Singleton<UIManager>
    {
        protected override void Awake()
        {
            base.Awake();

            WakeLevelUI();
            WakeShipUI();
            WakeWeaponUI();
            WakeOtherUI();
        }

        private void Start()
        {
            StartLevelUI();
            StartShipUI();
            StartWeaponUI();
            StartOtherUI();
        }

        #region Level UI
        [Header("Level UI")]
        [SerializeField] TextMeshProUGUI m_ScoreText = null;
        [SerializeField] float m_STextLerpRate = 0;
        [SerializeField] TextMeshProUGUI m_TimerText = null;

        private bool m_IsLevelUISetup = false;
        private bool m_IsSTextLerping = false;

        private void WakeLevelUI()
        {
            // Check if reference is setup
            if (m_ScoreText == null)
                Debug.LogError("Level UI: No Reference to score text!!");
            else if (m_TimerText == null)
                Debug.LogError("Level UI: No Reference to timer text!!");
            else
                m_IsLevelUISetup = true;
        }

        private void StartLevelUI()
        {
            if (m_IsLevelUISetup)
            {
                UpdateScoreText(0, 0);
                UpdateTimerText(0.0f);
            }
        }
        
        // TODO: Add comments
        public void UpdateScoreText(int _currentValue, int _targetValue)
        {
            if (m_IsLevelUISetup)
                StartCoroutine(CR_LerpScoreText(_currentValue, _targetValue));
        }

        private IEnumerator CR_LerpScoreText(int _from, int _to)
        {
            m_IsSTextLerping = true;
            float _timeLeft = 1;

            while (_timeLeft > 0)
            {
                m_ScoreText.text = Mathf.SmoothStep(_to, _from, _timeLeft).ToString("00000000");
                _timeLeft -= (Time.deltaTime / m_STextLerpRate);
                yield return null;
            }

            m_ScoreText.text = _to.ToString("00000000");
            m_IsSTextLerping = false;
        }

        public void UpdateTimerText(float _value)
        {
            if (m_IsLevelUISetup)
            {
                int _minute, _second;
                float _milisecond;

                _milisecond = (_value % 1) * 100;
                _milisecond = Mathf.Clamp(_milisecond, 0, 99);
                _second = ((int)_value) % 60;
                _minute = (int)_value / 60;

                m_TimerText.text = _minute.ToString("00") + ":" + _second.ToString("00") + ":" + _milisecond.ToString("00");
            }
        }
        #endregion

        #region Ship UI
        [Header("Ship UI")]
        [SerializeField] TextMeshProUGUI m_ShipName = null;
        [SerializeField] TextMeshProUGUI m_HealthValueText = null;
        [SerializeField] Gradient m_HealthBarColor = null;
        [SerializeField] Image m_PrimaryBar = null;
        [SerializeField] float m_PBarLerpRate = 0;
        [SerializeField] Image m_SecondaryBar = null;
        [SerializeField] float m_SBarLerpDelay = 0;

        private bool m_IsShipUISetup = false;
        private float m_TargetHValue = 0;
        private bool m_IsSBarLerping = false;
        private bool m_IsHVTextLerping = false;

        private void WakeShipUI()
        {
            // Check if reference is setup
            if (m_ShipName == null)
                Debug.LogError("Ship UI: No Reference to ship name!!");
            else if (m_PrimaryBar == null)
                Debug.LogError("Ship UI: No Reference to primary bar!!");
            else if (m_SecondaryBar == null)
                Debug.LogError("Ship UI: No Reference to secondary bar!!");
            else if (m_HealthValueText == null)
                Debug.LogError("Ship UI: No Reference to text value!!");
            else
                m_IsShipUISetup = true;
        }

        private void StartShipUI()
        {
            // Setup ship ui if reference to components are validated
            if (m_IsShipUISetup && m_ShipName.text != "")
            {
                m_PrimaryBar.fillAmount = 1;
                m_PrimaryBar.color = m_HealthBarColor.Evaluate(1);
                m_SecondaryBar.fillAmount = 1;
                m_HealthValueText.text = 1.ToString("000%");
            }
            else
                Debug.LogError("Ship UI: Ship name is not defined!!");
        }

        public void SetShipName(string _shipName)
        {
            m_ShipName.SetText(_shipName);
        }

        /// <summary>
        /// Update health ui from param given.
        /// </summary>
        public void UpdateHealthBar(float _currentHealth, float _totalHealth)
        {
            if (m_IsShipUISetup)
            {
                float _from = m_SecondaryBar.fillAmount;
                m_TargetHValue = _currentHealth / _totalHealth;

                // Update primary bar
                UpdatePrimaryBar();

                // Update secondary bar
                if (!m_IsSBarLerping)
                    StartCoroutine(CR_LerpSecondaryBar());

                // Update text value
                if (!m_IsHVTextLerping)
                    StartCoroutine(CR_LerpHealthTextValue(_from));
            }
        }

        #region UpdatePrimaryBar, CR_LerpSecondaryBar, CR_LerpHealthTextValue
        /// <summary>
        /// Update primaty bar to reflect current health
        /// </summary>
        private void UpdatePrimaryBar()
        {
            m_PrimaryBar.fillAmount = m_TargetHValue;
            m_PrimaryBar.color = m_HealthBarColor.Evaluate(m_PrimaryBar.fillAmount);
        }

        /// <summary>
        /// Update secondary bar to reflect damage taken after certain an delay.
        /// </summary>
        private IEnumerator CR_LerpSecondaryBar()
        {
            m_IsSBarLerping = true;
            yield return new WaitForSeconds(m_SBarLerpDelay);

            float _timeLeft = 1;
            float _from = m_SecondaryBar.fillAmount;

            while (_timeLeft > 0)
            {
                m_SecondaryBar.fillAmount = Mathf.SmoothStep(m_TargetHValue, _from, _timeLeft);
                _timeLeft -= (Time.deltaTime / m_PBarLerpRate);
                yield return null;
            }
            m_IsSBarLerping = false;
        }

        /// <summary>
        /// Update text value to animate the changes in health
        /// </summary>
        private IEnumerator CR_LerpHealthTextValue(float _from)
        {
            m_IsHVTextLerping = true;
            float _timeLeft = 1;

            while (_timeLeft > 0)
            {
                m_HealthValueText.text = Mathf.Clamp(Mathf.SmoothStep(m_TargetHValue, _from, _timeLeft), 0, 10).ToString("000%");
                _timeLeft -= (Time.deltaTime / m_PBarLerpRate);
                yield return null;
            }

            m_IsHVTextLerping = false;
        }
        #endregion
        #endregion

        #region Weapon UI
        [Header("Weapon UI")]
        [SerializeField] TextMeshProUGUI m_WeaponName;
        [SerializeField] TextMeshProUGUI m_HeatValue;
        [SerializeField] Image m_HeatBar;
        [SerializeField] Gradient m_HeatBarColor = null;
        [SerializeField] Animator m_HeatBarAnim = null;

        private bool m_IsWeaponUISetup = false;

        private void WakeWeaponUI()
        {
            // Check if reference is setup
            if (m_WeaponName == null)
                Debug.LogError("Weapon UI: No Reference to weapon name!!");
            else if (m_HeatValue == null)
                Debug.LogError("Weapon UI: No Reference to heat value!!");
            else if (m_HeatBar == null)
                Debug.LogError("Weapon UI: No Reference to heat bar!!");
            else if (m_HeatBarAnim == null)
                Debug.LogError("Weapon UI: No Reference to heat bar animator!!");
            else
                m_IsWeaponUISetup = true;
        }

        private void StartWeaponUI()
        {
            // Setup health ui if reference to components are validated
            if (m_IsWeaponUISetup)
            {
                m_HeatValue.text = 0.ToString("000%");
                m_HeatBar.fillAmount = 0;
                m_HeatBar.color = m_HeatBarColor.Evaluate(0);
            }
        }

        public void SetWeaponName(string _weaponName)
        {
            m_WeaponName.SetText(_weaponName);
        }

        public void UpdateHeatBar(float _value)
        {
            m_HeatBar.fillAmount = _value;
            _value = Mathf.Clamp(_value, 0, 100);
            m_HeatValue.SetText(_value.ToString("000%"));
            m_HeatBar.color = m_HeatBarColor.Evaluate(m_HeatBar.fillAmount);
        }
        #endregion

        #region Other UI
        [Header("Other UI")]
        [SerializeField] GameObject m_PauseMenu = null;
        [SerializeField] GameObject m_FinalScreen = null;

        private bool m_IsOtherUISetup = false;

        private void WakeOtherUI()
        {
            // Check if reference is setup
            if (m_PauseMenu == null)
                Debug.LogError("Other UI: No Reference to pause menu!!");
            else if (m_FinalScreen == null)
                Debug.LogError("Other UI: No Reference to round result UI!!");
            else
                m_IsOtherUISetup = true;
        }

        private void StartOtherUI()
        {
            // Setup health ui if reference to components are validated
            if (m_IsOtherUISetup)
            {
                m_PauseMenu.SetActive(false);
                m_FinalScreen.SetActive(false);
            }
        }

        /// <summary>
        /// Toggle Pause Menu by passing the state to this funtion
        /// </summary>
        /// <param name="_state">state</param>
        public void TogglePauseMenu(bool _state)
        {
            m_PauseMenu.SetActive(_state);
            Cursor.visible = !_state;
        }

        public void ActivateFinalScreen()
        {
            m_FinalScreen.SetActive(true);
        }
        #endregion

    }
}
