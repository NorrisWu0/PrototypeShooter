
using System.Collections;
using UnityEngine;
using TMPro;
using Cinemachine;

namespace GeoShot
{
    public class LevelManager : Singleton<LevelManager>
    {
        public bool isPlaying;

        [Header("Level Setting")]
        [SerializeField] Player m_Player;
        [SerializeField] int m_Score;
        [Range(0,1)]
        [SerializeField] float m_TimeScale;
        [SerializeField] float m_TimeElapsed;
        [SerializeField] Animator m_CompleteText;
        [SerializeField] Animator m_FailedText;

        [Header("TODO")]
        [SerializeField] MenuManager m_MenuManager;

        private void Update()
        {
            #region Record Time
            if (isPlaying)
            {
                m_TimeElapsed += Time.deltaTime * m_TimeScale;
                UIManager.Instance.UpdateTimerText(m_TimeElapsed);
            }
            #endregion

            if (Input.GetKeyDown(KeyCode.Escape))
                ToggleGamePause();
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
            Debug.Log("Updating ScoreText");
            UIManager.Instance.UpdateScoreText(m_Score, m_Score + _value);
            m_Score += _value;
        }

        public void EndLevel()
        {
            isPlaying = false;
        }
    }
}