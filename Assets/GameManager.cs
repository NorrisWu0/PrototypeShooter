using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Level Setting")]
    [SerializeField] GameObject m_Player;
    [SerializeField] float m_Timer;
    [SerializeField] TextMeshProUGUI m_TimerText;
    [SerializeField] Animator m_CompleteText;
    [SerializeField] Animator m_FailedText;
    
    [Header("Score System")]
    [SerializeField] float m_Score;
    [SerializeField] float m_TargetScore;
    [SerializeField] float m_LevelReward;
    [Range(0,1)]
    [SerializeField] float m_ScoreLerpRate;
    [SerializeField] TextMeshProUGUI m_ScoreText;

    [Header("TODO")]
    [SerializeField] MenuManager m_MenuManager;

    void Start()
    {
        m_Player = GameObject.FindWithTag("Player");

        if (instance == null)
            instance = this;

        StartCoroutine(CheckPlayerStatus());
        StartCoroutine(UpdateScoreUI());
        StartCoroutine(StartTimer());
    }

    public void AddScore(float _score)
    {
        Debug.Log(_score);
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

    #region Check if player is alive - Need to find somewhere else for this
    IEnumerator CheckPlayerStatus()
    {
        while (true)
        {
            if (!m_Player.activeSelf)
            {
                m_FailedText.SetTrigger("Fade");
                yield return new WaitForSeconds(5f);
                m_MenuManager.ExitLevel();
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
    #endregion

    #region Timer
    IEnumerator StartTimer()
    {
        while (m_Timer >= 0)
        {
            m_Timer -= Time.deltaTime;
            
            #region Update Timer UI
            float _minutes = Mathf.Clamp(Mathf.Floor(m_Timer / 60), 0, Mathf.Infinity);
            float _seconds = Mathf.Clamp(Mathf.RoundToInt(m_Timer % 60), 0, Mathf.Infinity);

            m_TimerText.SetText(_minutes.ToString("00") + ":" + _seconds.ToString("00"));
            #endregion
            
            yield return null;
        }

        StartCoroutine(ExitLevel());
    }
    #endregion

    #region Exit Level
    IEnumerator ExitLevel()
    {
        m_TargetScore += m_LevelReward;
        m_CompleteText.SetTrigger("Fade");
        string _levelName = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetInt(_levelName, Mathf.RoundToInt(m_TargetScore));
        PlayerPrefs.SetInt("credits", PlayerPrefs.GetInt("credits") + Mathf.RoundToInt(m_Score));
        yield return new WaitForSeconds(5f);
        m_MenuManager.ExitLevel();
    }
    #endregion

}
