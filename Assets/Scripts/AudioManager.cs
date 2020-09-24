using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeShooter
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
