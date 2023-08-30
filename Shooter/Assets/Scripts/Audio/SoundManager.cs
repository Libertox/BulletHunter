using UnityEngine;

namespace BulletHaunter
{
    public class SoundManager: MonoBehaviour
    {
        public static SoundManager Instance { get; private set; }

        private const string PLAYER_PREFS_SOUND_EFFECT_VOLUME = "SoundEffectVolume";
        private readonly float defaultSoundEffectVolume = .5f;

        [SerializeField] private AudioSource soundEffectSource;
        [SerializeField] private AudioClipsSO audioClipsSO;

        public float SoundEffectVolume { get; private set; }

       
        private void Awake()
        {
            if (!Instance)
                Instance = this;

            SoundEffectVolume = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECT_VOLUME, defaultSoundEffectVolume);
        }

        private void Start() => PlayerController.OnJumped += PlayerController_OnJumped;

        private void OnDestroy() => PlayerController.OnJumped -= PlayerController_OnJumped;
      

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

        private void PlaySoundEffect(AudioClip audioClip, Vector3 audioEffectPosition) 
        {
            AudioSource.PlayClipAtPoint(audioClip, audioEffectPosition, SoundEffectVolume);
        }

        private void PlaySoundEffect(AudioClip[] audioClips, Vector3 audioEffectPosition)
        {
            int audioClipIndex = Random.Range(0, audioClips.Length);
            AudioSource.PlayClipAtPoint(audioClips[audioClipIndex], audioEffectPosition, SoundEffectVolume);
        }

        public void PlayButtonSound()
        {
            int buttonClipIndex = Random.Range(0, audioClipsSO.ButtonSound.Length);
            soundEffectSource.PlayOneShot(audioClipsSO.ButtonSound[buttonClipIndex],SoundEffectVolume);
        }

        public void PlayShootSound(Vector3 audioSourcePosition) => 
            PlaySoundEffect(audioClipsSO.ShootSound, audioSourcePosition);

        public void PlayBulletImpactSound(Vector3 audioSourcePosition) => 
            PlaySoundEffect(audioClipsSO.BulletImpactSound, audioSourcePosition);

        public void PlayGrenadeExplosionSound(Vector3 audioSourcePosition) => 
            PlaySoundEffect(audioClipsSO.ExplosionSound, audioSourcePosition);

        public void PlayPlayerWalkSound(Vector3 audioSourcePosition) => 
            PlaySoundEffect(audioClipsSO.WalkSound, audioSourcePosition);
      
        public void PlayPlayerJumpSound(Vector3 audioSourcePosition) => 
            PlaySoundEffect(audioClipsSO.JumpSound, audioSourcePosition);
      
        public void PlayPickupObjectSound(Vector3 audioSourcePosition) => 
            PlaySoundEffect(audioClipsSO.PickupObjectSound, audioSourcePosition);
  
        public void PlayPlayerTakeDamageSound(Vector3 audioSourcePosition) => 
            PlaySoundEffect(audioClipsSO.TakeDamageSound, audioSourcePosition);
    }
}
