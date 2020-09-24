using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace PrototypeShooter
{
    public class MenuManager : MonoBehaviour
    {
        [Header("Loading Screen Setting")]
        [SerializeField] GameObject m_LoadingMaskUI;
        [SerializeField] TextMeshProUGUI m_NextSceneName;
        [SerializeField] Slider m_LoadingBar;
        
        #region Loading
        /// <summary>
        /// Switch to a given scene and reset timeScale to 1 just in case the timescale was modified in other scene right before switches.
        /// </summary>
        public void StartLevel(string _sceneName)
        {
            StartCoroutine(LoadingLevel(_sceneName));
            Time.timeScale = 1;

            if (_sceneName == "MainMenu")
                Cursor.visible = true;
        }

        /// <summary>
        /// The actual loading operation
        /// </summary>
        IEnumerator LoadingLevel(string _sceneName)
        {
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            AsyncOperation _operation = SceneManager.LoadSceneAsync(_sceneName);

            m_NextSceneName.SetText("Loading " + _sceneName + "...");
            m_LoadingMaskUI.SetActive(true);
        

            while (!_operation.isDone)
            {
                float _progress = Mathf.Clamp01(_operation.progress / 0.9f);
            
                m_LoadingBar.value = _progress; 

                yield return null;
            }
        }
        #endregion

        /// <summary>
        /// This is pretty self-explanatory
        /// </summary>
        public void ExitGame()
        {
            Application.Quit();
        }
    }
}