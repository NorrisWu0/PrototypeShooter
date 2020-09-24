
using System.Collections;
using UnityEngine;
using TMPro;
using Cinemachine;

namespace PrototypeShooter
{
    public class LevelManager : Singleton<LevelManager>
    {
        public bool isPlaying = false;
        public bool isEnded = false;

        [Header("Level Setting")]
        [SerializeField] Player m_Player = null;
        [SerializeField] int m_Score = 0;
        [Range(0,1)]
        [SerializeField] float m_TimeScale = 1;
        [SerializeField] float m_TimeElapsed = 0;
        [SerializeField] Animator m_CompleteText = null;
        [SerializeField] Animator m_FailedText = null;

        [Header("TODO")]
        [SerializeField] MenuManager m_MenuManager = null;

        private void Update()
        {
            #region Record Time
            if (isPlaying)
            {
                m_TimeElapsed += Time.deltaTime * m_TimeScale;
                UIManager.Instance.UpdateTimerText(m_TimeElapsed);
            }
            #endregion

            if (Input.GetKeyDown(KeyCode.Escape) && !isEnded)
                ToggleGamePause();
            else if (Input.GetKeyDown(KeyCode.Escape) && isEnded)
                m_MenuManager.StartLevel("MainMenu");

        }

        public void ToggleGamePause() 
        {
            isPlaying = !isPlaying;
            UIManager.Instance.TogglePauseMenu(!isPlaying);
            
            if (isPlaying)
            {
                AudioManager.Instance.backgroundAudioSource.volume = 0.8f;
                Time.timeScale = 1.0f;
            }
            else
            {
                AudioManager.Instance.backgroundAudioSource.volume = 0.2f;
                Time.timeScale = 0.0f;
            }
        }

        public void UpdateScore(int _value)
        {
            UIManager.Instance.UpdateScoreText(m_Score, m_Score + _value);
            m_Score += _value;
        }

        public void EndLevel()
        {
            isPlaying = false;
            isEnded = true;
            UIManager.Instance.ActivateFinalScreen();
        }
    }
}