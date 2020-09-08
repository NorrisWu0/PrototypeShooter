using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

namespace GeoShot
{
    public class HealthUI : MonoBehaviour
    {
        [Header("Health UI")]
        [SerializeField] TextMeshProUGUI m_TextValue = null;
        [SerializeField] Gradient m_ColorIndicator = null;
        [SerializeField] Image m_PrimaryBar = null;
        [SerializeField] float m_PBarLerpRate = 0;
        [SerializeField] Image m_SecondaryBar = null;
        [SerializeField] float m_SBarLerpDelay = 0;

        private bool m_IsSetup = false;
        private float m_TargetValue = 0;
        private bool m_IsSBarLerping = false;
        private bool m_IsTValueLerping = false;

        private void Awake()
        {
            // Check if reference is setup
            if (m_PrimaryBar == null)
                Debug.LogError("Health UI - No Reference to primary bar!!");
            else if (m_SecondaryBar == null)
                Debug.LogError("Health UI - No Reference to secondary bar!!");
            else if (m_TextValue == null)
                Debug.LogError("Health UI - No Reference to text value!!");
            else
                m_IsSetup = true;
        }

        private void Start()
        {
            if (m_IsSetup)
            {
                m_PrimaryBar.fillAmount = 1;
                m_PrimaryBar.color = m_ColorIndicator.Evaluate(1);
                m_SecondaryBar.fillAmount = 1;
                m_TextValue.text = 1.ToString("000%");
            }
        }

        /// <summary>
        /// Update health ui from param given.
        /// </summary>
        public void UpdateHealthUI(float _currentHealth, float _totalHealth)
        {
            if (m_IsSetup)
            {
                float _from = m_SecondaryBar.fillAmount;
                m_TargetValue = _currentHealth / _totalHealth;

                // Update primary bar
                UpdatePrimaryBar();

                // Update secondary bar
                if (!m_IsSBarLerping)
                    StartCoroutine(CR_UpdateSecondaryBar());

                // Update text value
                if (!m_IsTValueLerping)
                    StartCoroutine(CR_UpdateTextValue(_from));
            }
        }

        #region UpdateHealthBar, UpdateFillBar, CR_UpdateFadeBar, CR_UpdateFadeBar
        /// <summary>
        /// Update primaty bar to reflect current health
        /// </summary>
        private void UpdatePrimaryBar()
        {
            m_PrimaryBar.fillAmount = m_TargetValue;
            m_PrimaryBar.color = m_ColorIndicator.Evaluate(m_PrimaryBar.fillAmount);
        }

        /// <summary>
        /// Update secondary bar to reflect damage taken after certain an delay.
        /// </summary>
        private IEnumerator CR_UpdateSecondaryBar()
        {
            m_IsSBarLerping = true;
            yield return new WaitForSeconds(m_SBarLerpDelay);
            
            float _timeLeft = 1;
            float _from = m_SecondaryBar.fillAmount;
            
            while (_timeLeft > 0)
            {
                m_SecondaryBar.fillAmount = Mathf.SmoothStep(m_TargetValue, _from, _timeLeft);
                _timeLeft -= (Time.deltaTime / m_PBarLerpRate);
                yield return null;
            }
            m_IsSBarLerping = false;
        }

        /// <summary>
        /// Update text value to animate the changes in health
        /// </summary>
        private IEnumerator CR_UpdateTextValue(float _from)
        {
            m_IsTValueLerping = true;
            float _timeLeft = 1;

            while (_timeLeft > 0)
            {
                m_TextValue.text = Mathf.Clamp(Mathf.SmoothStep(m_TargetValue, _from, _timeLeft), 0, 10).ToString("000%");
                _timeLeft -= (Time.deltaTime / m_PBarLerpRate);
                yield return null;
            }

            m_IsTValueLerping = false;
        }
        #endregion
    }
}