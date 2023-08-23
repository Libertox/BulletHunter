using UnityEngine;

namespace BulletHaunter
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

        private void Start() => PlayerController.OnJumped += PlayerController_OnJumped;
      
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
            int buttonClipIndex = Random.Range(0, audioClipsSO.ButtonSound.Length);
            soundEffectSource.PlayOneShot(audioClipsSO.ButtonSound[buttonClipIndex],SoundEffectVolume);
        }



        public void PlayShootSound(Vector3 audioSourcePosition)
        {
            int shootClipIndex = Random.Range(0, audioClipsSO.ShootSound.Length);
            AudioSource.PlayClipAtPoint(audioClipsSO.ShootSound[shootClipIndex], audioSourcePosition, SoundEffectVolume);
        }

        public void PlayBulletImpactSound(Vector3 audioSourcePosition)
        {
            int bulletImpactClipIndex = Random.Range(0, audioClipsSO.BulletImpactSound.Length);
            AudioSource.PlayClipAtPoint(audioClipsSO.BulletImpactSound[bulletImpactClipIndex], audioSourcePosition, SoundEffectVolume);
        }

        public void PlayGrenadeExplosionSound(Vector3 audioSourcePosition)
        {
            int explosionClipIndex = Random.Range(0, audioClipsSO.ExplosionSound.Length);
            AudioSource.PlayClipAtPoint(audioClipsSO.ExplosionSound[explosionClipIndex], audioSourcePosition, SoundEffectVolume);
        }
     
        public void PlayPlayerWalkSound(Vector3 audioSourcePosition) => AudioSource.PlayClipAtPoint(audioClipsSO.WalkSound, audioSourcePosition, SoundEffectVolume);
 
        public void PlayPlayerJumpSound(Vector3 audioSourcePosition) => AudioSource.PlayClipAtPoint(audioClipsSO.JumpSound, audioSourcePosition, SoundEffectVolume);
   
        public void PlayPickupObjectSound(Vector3 audioSourcePosition) => AudioSource.PlayClipAtPoint(audioClipsSO.PickupObjectSound, audioSourcePosition, SoundEffectVolume);
  
        public void PlayPlayerTakeDamageSound(Vector3 audioSourcePosition) => AudioSource.PlayClipAtPoint(audioClipsSO.TakeDamageSound, audioSourcePosition, SoundEffectVolume);
    }
}
