using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public PlayerData PlayerData;

    [Header("Level Setting")]
    [SerializeField] GameObject m_LevelUI;
    [SerializeField] int m_BuildIndex;
    [SerializeField] TextMeshProUGUI m_LevelText;

    [Header("Hanger Setting")]
    [SerializeField] GameObject m_HangerUI;
    [SerializeField] int m_Credits;
    [SerializeField] float m_MaxHealth;
    [SerializeField] float m_FireRate;
    [SerializeField] float m_DisperseValue;

    [SerializeField] TextMeshProUGUI m_CreditText;
    [SerializeField] TextMeshProUGUI m_MaxHealthText;
    [SerializeField] TextMeshProUGUI m_FireRateText;
    [SerializeField] TextMeshProUGUI m_DisperseValueText;

    [Header("DEBUG SETTING")]
    [SerializeField] int m_InitCredits;
    [SerializeField] float m_InitMaxHealth;
    [SerializeField] float m_InitFireRate;
    [SerializeField] float m_InitDisperseValue;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("credits"))
            InitializePlayerData();

        UpdateDataToUI();
    }
    
    public void ShowLevels()
    {
        m_LevelUI.SetActive(true);
        m_HangerUI.SetActive(false);
    }
    
    // NEED TO BUILD LEVEL FROM SO!!!!!!!!!!
    public void SelectLevel(int _buildIndex)
    {
        m_BuildIndex = _buildIndex;
        m_LevelText.SetText("Level " + m_BuildIndex);
    }

    public void StartLevel()
    {
        if (m_BuildIndex != 0)
        {
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            SceneManager.LoadSceneAsync(m_BuildIndex);
        }
    }

    public void ShowHanger()
    {
        m_LevelUI.SetActive(false);
        m_HangerUI.SetActive(true);
    }
    
    public void UpgradeHealth(int _cost)
    {
        if (m_Credits >= _cost)
        {
            m_Credits -= _cost;
            m_MaxHealth *= 1.1f;
            m_CreditText.SetText("Credits: $" + m_Credits.ToString());
            m_MaxHealthText.SetText("Max Health: " + m_MaxHealth.ToString(".00"));
        }
    }

    public void UpgradeFireRate(int _cost)
    {
        if (m_Credits >= _cost)
        {
            m_Credits -= _cost;
            m_FireRate *= 0.9f;
            m_DisperseValue *= 1.05f;
            m_CreditText.SetText("Credits: $" + m_Credits.ToString());
            m_FireRateText.SetText("Fire Rate: " + Mathf.RoundToInt(60 / m_FireRate).ToString() + " RPM");
            m_DisperseValueText.SetText("Dispersion: " + m_DisperseValue.ToString(".00") + " Degree");
        }
    }
    public void UpgradeDispersion(int _cost)
    {
        if (m_Credits >= _cost)
        {
            m_Credits -= _cost;
            m_DisperseValue *= 0.9f;
            m_CreditText.SetText("Credits: $" + m_Credits.ToString());
            m_DisperseValueText.SetText("Dispersion: " + m_DisperseValue.ToString(".00") + " Degree");
        }
    }

    public void ConfirmUpgrade()
    {
        PlayerPrefs.SetInt("credits", m_Credits);
        PlayerPrefs.SetFloat("maxHealth", m_MaxHealth);
        PlayerPrefs.SetFloat("fireRate", m_FireRate);
        PlayerPrefs.SetFloat("disperseRate", m_DisperseValue);
    }

    public void ResetUpgrade()
    {
        UpdateDataToUI();
    }

    public void InitializePlayerData()
    {
        PlayerPrefs.SetInt("credits", m_InitCredits);
        PlayerPrefs.SetFloat("maxHealth", m_InitMaxHealth);
        PlayerPrefs.SetFloat("fireRate", m_InitFireRate);
        PlayerPrefs.SetFloat("disperseRate", m_InitDisperseValue);

        UpdateDataToUI();
    }

    public void MeanStats()
    {
        PlayerPrefs.SetInt("credits", 10000);
        PlayerPrefs.SetFloat("maxHealth", 200);
        PlayerPrefs.SetFloat("fireRate", 0.5f);
        PlayerPrefs.SetFloat("disperseRate", 2f);

        UpdateDataToUI();
    }

    void UpdateDataToUI()
    {
        PlayerData = PlayerPersistence.LoadData();

        m_Credits = PlayerData.credits;
        m_MaxHealth = PlayerData.maxHealth;
        m_FireRate = PlayerData.fireRate;
        m_DisperseValue = PlayerData.disperseValue;

        m_CreditText.SetText("Credits: $" + m_Credits.ToString());
        m_MaxHealthText.SetText("Max Health: " + m_MaxHealth.ToString(".00"));
        m_FireRateText.SetText("Fire Rate: " + Mathf.RoundToInt(60 / m_FireRate).ToString() + " RPM");
        m_DisperseValueText.SetText("Dispersion: " + m_DisperseValue.ToString(".00") + " Degree");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
