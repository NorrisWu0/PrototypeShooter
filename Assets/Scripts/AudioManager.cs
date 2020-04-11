using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GeoShot
{
    public class AudioManager : Singleton<AudioManager>
    {
        public AudioSource backgroundAudioSource;
        public AudioSource weaponAudioSource;
        public AudioSource enemyAudioSource;
        public AudioSource effectAudioSource;
        public AudioSource playerAudioSource;
    }
}
