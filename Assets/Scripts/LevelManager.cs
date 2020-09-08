
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
            m_TimeElapsed += Time.deltaTime * m_TimeScale;
            UIManager.Instance.UpdateTimerText(m_TimeElapsed);
        }

        public void UpdateScore(int _value)
        {
            Debug.Log("Updating ScoreText");
            UIManager.Instance.UpdateScoreText(m_Score, m_Score + _value);
            m_Score += _value;
        }
    }
}