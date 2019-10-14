using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerPersistence
{
    public static void SaveData(Player _player)
    {
        PlayerPrefs.SetFloat("maxHealth", _player.maxHealth);
        PlayerPrefs.SetFloat("fireRate", _player.fireRate);
        PlayerPrefs.SetFloat("disperseRate", _player.disperseRate);
    }

    public static PlayerData LoadData()
    {
        int _credit = PlayerPrefs.GetInt("credits");
        float _maxHealth = PlayerPrefs.GetFloat("maxHealth");
        float _fireRate = PlayerPrefs.GetFloat("fireRate");
        float _disperseRate = PlayerPrefs.GetFloat("disperseRate");

        PlayerData _playerData = new PlayerData()
        {
            credits = _credit,
            maxHealth = _maxHealth,
            fireRate = _fireRate,
            disperseValue = _disperseRate
        };

        return _playerData;
    }

}
