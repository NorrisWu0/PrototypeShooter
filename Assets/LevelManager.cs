﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Cinemachine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [Header("Level Setting")]
    [SerializeField] Player m_Player;
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
    [SerializeField] CinemachineVirtualCamera m_VirtualCamera;
    [SerializeField] float m_Amplitude;
    [SerializeField] float m_Frequency;

    private CinemachineBasicMultiChannelPerlin m_VCameraNoise;

    void Start()
    {
        if (m_Player == null)
            m_Player = GameObject.FindWithTag("Player").GetComponent<Player>();

        if (instance == null)
            instance = this;

        m_VCameraNoise = m_VirtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();

        StartCoroutine(CheckPlayerStatus());
        StartCoroutine(UpdateScoreUI());
        StartCoroutine(StartTimer());
    }

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

    #region Check if player is alive - Need to find somewhere else for this
    IEnumerator CheckPlayerStatus()
    {
        while (m_Player.isAlive)
        {
            yield return null;
        }

        StartCoroutine(ShakeCamera());
        m_FailedText.SetTrigger("Fade");
        if (m_TargetScore != 0)
            HighScoreTable.instance.AddEntry(Mathf.RoundToInt(m_TargetScore), "TODO");
    }
    #endregion

    IEnumerator ShakeCamera()
    {
        float _t = 1;
        while (_t >= 0)
        {
            m_VCameraNoise.m_AmplitudeGain = Mathf.Lerp(0, m_Amplitude, _t);
            m_VCameraNoise.m_FrequencyGain = Mathf.Lerp(0, m_Frequency, _t);
            _t -= Time.deltaTime;
            yield return null;
        }
    }

    #region Timer
    IEnumerator StartTimer()
    {
        while (m_Player.isAlive)
        {
            m_Timer += Time.deltaTime;
            
            #region Update Timer UI
            string _minutes = (m_Timer / 60).ToString("00");
            string _seconds = (m_Timer % 60).ToString("00.00");

            m_TimerText.SetText(_minutes + ":" + _seconds);
            #endregion
            
            yield return null;
        }
    }
    #endregion
}