using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [Header("Pause Menu Setting")]
    [SerializeField] GameObject m_PauseMenuUI;
    [SerializeField] bool m_IsPaused;

    [Header("Loading Screen Setting")]
    [SerializeField] GameObject m_LoadingMaskUI;
    [SerializeField] TextMeshProUGUI m_NextSceneName;
    [SerializeField] Slider m_LoadingBar;

    private void Start()
    {
        if (m_PauseMenuUI != null)
            StartCoroutine(PauseMenu());
        else
        {
            Cursor.visible = true;
        }
    }

    private void Update()
    {
        if (m_PauseMenuUI != null)
            if (Input.GetKeyDown(KeyCode.Escape))
                m_IsPaused = !m_IsPaused;
    }

    #region Pause Menu
    IEnumerator PauseMenu()
    {
        while (true)
        {
            while (m_IsPaused)
            {
                Time.timeScale = 0;
                Cursor.visible = true;
                AudioManager.instance.backgroundAudioSource.volume = 0.2f;
                m_PauseMenuUI.SetActive(true);

                yield return null;
            }

            Time.timeScale = 1;
            Cursor.visible = false;
            AudioManager.instance.backgroundAudioSource.volume = 0.8f;
            m_PauseMenuUI.SetActive(false);

            yield return null;
        }
    }

    public void TogglePauseState(bool _state)
    {
        m_IsPaused = _state;
    }
    #endregion

    #region Loading
    public void StartLevel(string _sceneName)
    {
        StartCoroutine(LoadingLevel(_sceneName));
    }

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
    
    public void ExitGame()
    {
        Application.Quit();
    }

}