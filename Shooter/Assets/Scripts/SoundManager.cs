using System;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Shooter
{
    public class SoundManager: MonoBehaviour
    {
        public static SoundManager Instance { get; private set; }

        private const string PLAYER_PREFS_SOUND_EFFECT_VOLUME = "SoundEffectVolume";
        private readonly float defaultSoundEffectVolume = .5f;

        public float SoundEffectVolume { get; private set; }

        [SerializeField] private AudioSource soundEffectSource;
        [SerializeField] private AudioClipsSO audioClipsSO;

        private void Awake()
        {
            if (!Instance)
                Instance = this;

            SoundEffectVolume = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECT_VOLUME, defaultSoundEffectVolume);
        }

        private void Start()
        {
            PlayerController.OnJumped += PlayerController_OnJumped;
        }

        private void PlayerController_OnJumped(object sender, PlayerController.OnStateChangedEventArgs e)
        {
            if (!e.state) return;

            PlayerController playerController = sender as PlayerController;
            PlayPlayerJumpSound(playerController.transform.position);
        }

        public void SetSoundEffectVolume(float volume)
        {
            SoundEffectVolume = volume;
            PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECT_VOLUME, volume);
            PlayerPrefs.Save();
        }

        public void PlayButtonSound()
        {
            int buttonClipIndex = UnityEngine.Random.Range(0, audioClipsSO.ButtonSound.Length);
            soundEffectSource.PlayOneShot(audioClipsSO.ButtonSound[buttonClipIndex],SoundEffectVolume);
        }

        public void PlayShootSound(Vector3 playPoint)
        {
            int shootClipIndex = UnityEngine.Random.Range(0, audioClipsSO.ShootSound.Length);
            AudioSource.PlayClipAtPoint(audioClipsSO.ShootSound[shootClipIndex], playPoint, SoundEffectVolume);
        }

        public void PlayBulletImpactSound(Vector3 playPoint)
        {
            int bulletImpactClipIndex = UnityEngine.Random.Range(0, audioClipsSO.BulletImpactSound.Length);
            AudioSource.PlayClipAtPoint(audioClipsSO.BulletImpactSound[bulletImpactClipIndex], playPoint, SoundEffectVolume);
        }

        public void PlayGrenadeExplosionSound(Vector3 playPoint)
        {
            int explosionClipIndex = UnityEngine.Random.Range(0, audioClipsSO.ExplosionSound.Length);
            AudioSource.PlayClipAtPoint(audioClipsSO.ExplosionSound[explosionClipIndex], playPoint, SoundEffectVolume);
        }
     
        public void PlayPlayerWalkSound(Vector3 playPoint)
        {
            AudioSource.PlayClipAtPoint(audioClipsSO.WalkSound, playPoint, SoundEffectVolume);
        }

        public void PlayPlayerJumpSound(Vector3 playPoint)
        {
            AudioSource.PlayClipAtPoint(audioClipsSO.JumpSound, playPoint, SoundEffectVolume);
        }

        public void PlayPickupObjectSound(Vector3 playpoint)
        {
            AudioSource.PlayClipAtPoint(audioClipsSO.PickupObjectSound, playpoint, SoundEffectVolume);
        }

        public void PlayPlayerTakeDamageSound(Vector3 playPoint)
        {
            AudioSource.PlayClipAtPoint(audioClipsSO.TakeDamageSound, playPoint, SoundEffectVolume);
        }
    }
}
