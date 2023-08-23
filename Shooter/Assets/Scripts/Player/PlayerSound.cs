using UnityEngine;

namespace BulletHaunter
{
    public class PlayerSound:MonoBehaviour
    {
        private bool playWalkSound;
        private float footstepTimer;
        private readonly float footstepTimerMax = .25f;


        private void Start() => PlayerController.OnWalked += PlayerController_OnWalked;
      
        private void PlayerController_OnWalked(object sender, PlayerController.OnStateChangedEventArgs e) => playWalkSound = e.state;
      
        private void Update()
        {
            footstepTimer -= Time.deltaTime;
            if (footstepTimer < 0f)
            {
                footstepTimer = footstepTimerMax;
                if (playWalkSound)
                    SoundManager.Instance.PlayPlayerWalkSound(transform.position); 
            }
        }
    }
}
