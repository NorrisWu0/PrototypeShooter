using System.Collections;
using UnityEngine;
using TMPro;

namespace PrototypeShooter
{
    public class ScoreManager : Singleton<ScoreManager>
    {
        [Header("Score System")]
        [SerializeField] float m_Score = 0;
        [SerializeField] float m_TargetScore = 0;
        [SerializeField] float m_LevelReward = 0;
        [Range(0, 1)]
        [SerializeField] float m_ScoreLerpRate = 0;
        [SerializeField] TextMeshProUGUI m_ScoreText = null;


        public void AddScore(float _score)
        {
            m_TargetScore += _score;
        }

        IEnumerator UpdateScoreUI()
        {
            while (true)
            {
                while (m_Score != m_TargetScore)
                {
                    m_Score = Mathf.Lerp(m_Score, m_TargetScore, m_ScoreLerpRate);
                    m_ScoreText.SetText(m_Score.ToString("00000000"));
                    yield return null;
                }
                yield return null;
            }
        }

    }
}
