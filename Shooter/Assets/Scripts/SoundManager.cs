using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shooter
{
    public class SoundManager: MonoBehaviour
    {
        public static SoundManager Instance { get; private set; }

        private const string PLAYER_PREFS_SOUND_EFFECT_VOLUME = "SoundEffectVolume";
        private readonly float defaultSoundEffectVolume = .5f;

        public float SoundEffectVolume { get; private set; }

        [SerializeField] private AudioSource soundEffectSource;

        private void Awake()
        {
            if (!Instance)
                Instance = this;

            SoundEffectVolume = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECT_VOLUME, defaultSoundEffectVolume);
        }

        public void SetSoundEffectVolume(float volume)
        {
            SoundEffectVolume = volume;
            PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECT_VOLUME, volume);
            PlayerPrefs.Save();
        }


    }
}
