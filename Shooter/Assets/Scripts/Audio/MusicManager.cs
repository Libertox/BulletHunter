﻿using UnityEngine;

namespace BulletHaunter
{
    public class MusicManager:MonoBehaviour
    {
        public static MusicManager Instance { get; private set; }

        private const string PLAYER_PREFS_MUSIC_VOLUME = "MusicVolume";

        private readonly float volumeScale = 10f;
        private readonly float defaultMusicVolume = .5f;

        [SerializeField] private AudioSource musicSource;

        public float MusicVolume => musicSource.volume * volumeScale;

        private void Awake()
        {
            if (!Instance)
                Instance = this;

            musicSource.volume = PlayerPrefs.GetFloat(PLAYER_PREFS_MUSIC_VOLUME, defaultMusicVolume);
        }

        public void SetMusicVolume(float volume)
        {
            musicSource.volume = volume;
            PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC_VOLUME, volume);
            PlayerPrefs.Save();
        }

    }
}
