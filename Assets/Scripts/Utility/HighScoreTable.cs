using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HighScoreTable : MonoBehaviour
{
    public static HighScoreTable instance;

    [SerializeField] Transform m_ScoreContainer;
    [SerializeField] Transform m_ScoreEntryTemplate;
    [SerializeField] List<HighScoreEntry> m_HighScoreEntries;

    [System.Serializable]
    private class HighScoreEntry
    {
        public int ranking;
        public int score;
        public string name;
    }
    
    private class HighScores
    {
        public List<HighScoreEntry> highScoreEntries;
    }

    private void Awake()
    {
        //PlayerPrefs.DeleteAll();

        #region Singleton Test
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        #endregion




        SetupScoreboard();
        
    }
    
    public void SetupScoreboard()
    {
        m_ScoreContainer = GameObject.Find("Score Container").transform;
        m_ScoreEntryTemplate = GameObject.Find("Score Template").transform;
        m_ScoreEntryTemplate.gameObject.SetActive(false);

        string _jsonString = PlayerPrefs.GetString("highScoreEntries");
        HighScores _highScores = JsonUtility.FromJson<HighScores>(_jsonString);

        if (_highScores.highScoreEntries == null)
            m_HighScoreEntries = _highScores.highScoreEntries;
        else
            m_HighScoreEntries = new List<HighScoreEntry>{ new HighScoreEntry{ ranking = 0, score = 10000, name = "FMT"} };

        for (int i = 0; i < m_HighScoreEntries.Count; i++)
        {
            for (int j = i + 1; j < m_HighScoreEntries.Count; j++)
            {
                if (m_HighScoreEntries[j].score > m_HighScoreEntries[i].score)
                {
                    HighScoreEntry _temp = m_HighScoreEntries[i];
                    m_HighScoreEntries[i] = m_HighScoreEntries[j];
                    m_HighScoreEntries[i].ranking = i + 1;
                    m_HighScoreEntries[j] = _temp;
                    m_HighScoreEntries[j].ranking = j + 1;
                }
            }
        }

        foreach (HighScoreEntry _entry in m_HighScoreEntries)
        {
            CreateEntry(_entry, m_ScoreContainer);
        }

        if (_highScores.highScoreEntries == null)
        {
            HighScores _nhighScores = new HighScores { highScoreEntries = m_HighScoreEntries };
            string _json = JsonUtility.ToJson(_nhighScores);
            PlayerPrefs.SetString("highScoreEntries", _json);
            PlayerPrefs.Save();
        }

    }

    public void AddEntry(int _score, string _name)
    {
        HighScoreEntry _newEntry = new HighScoreEntry { score = _score, name = _name };

        string _jsonString = PlayerPrefs.GetString("highScoreEntries");
        HighScores _highScores = JsonUtility.FromJson<HighScores>(_jsonString);

        _highScores.highScoreEntries.Add(_newEntry);

        string _json = JsonUtility.ToJson(_highScores);
        PlayerPrefs.SetString("highScoreEntries", _json);
        PlayerPrefs.Save();
    }

    private void CreateEntry(HighScoreEntry _entry, Transform _container)
    {
        Transform _entryTransform = Instantiate(m_ScoreEntryTemplate, _container);

        _entryTransform.Find("Ranking").GetComponent<TextMeshProUGUI>().SetText(_entry.ranking.ToString("000"));
        _entryTransform.Find("Score").GetComponent<TextMeshProUGUI>().SetText(_entry.score.ToString("00000000"));
        _entryTransform.Find("Name").GetComponent<TextMeshProUGUI>().SetText(_entry.name);
        _entryTransform.gameObject.SetActive(true);
    }

}
