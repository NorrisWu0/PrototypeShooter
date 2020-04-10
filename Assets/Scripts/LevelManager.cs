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
        [SerializeField] float m_Timer;
        [SerializeField] TextMeshProUGUI m_TimerText;
        [SerializeField] Animator m_CompleteText;
        [SerializeField] Animator m_FailedText;

        [Header("TODO")]
        [SerializeField] MenuManager m_MenuManager;
        
        #region Timer
        IEnumerator StartTimer()
        {
            while (true)
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
}