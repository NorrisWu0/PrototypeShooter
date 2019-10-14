using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource weaponAudioSource;
    public AudioSource enemyAudioSource;
    public AudioSource playerAudioSource;

    private void Start()
    {
        if (instance == null)
            instance = this;
    }
}
