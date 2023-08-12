using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BulletHaunter
{
    public class RestorePlayerTimerUI:MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI restoreTimerText;

        private void Start()
        {
            if(PlayerStats.Instance != null)
            {
                PlayerStats.Instance.OnDeathed += PlayerStats_OnDeathed;
                PlayerStats.Instance.OnRestored += PlayersStats_OnRestored;
                PlayerStats.Instance.OnRestoreWaited += PlayerStats_OnRestoreWaited;
            }
            else
            {
                PlayerStats.OnAnyPlayerSpawn += PlayerStats_OnAnyPlayerSpawn;
            }

            Hide();
        }

     
        private void PlayerStats_OnAnyPlayerSpawn(object sender, EventArgs e)
        {
            if (PlayerStats.Instance != null)
            {
                PlayerStats.Instance.OnDeathed -= PlayerStats_OnDeathed;
                PlayerStats.Instance.OnDeathed += PlayerStats_OnDeathed;

                PlayerStats.Instance.OnRestoreWaited -= PlayerStats_OnRestoreWaited;
                PlayerStats.Instance.OnRestoreWaited += PlayerStats_OnRestoreWaited;

                PlayerStats.Instance.OnRestored -= PlayersStats_OnRestored;
                PlayerStats.Instance.OnRestored += PlayersStats_OnRestored;
            }
        }

        private void PlayerStats_OnRestoreWaited(object sender, PlayerStats.OnWaitedEventArgs e)
        {
            restoreTimerText.SetText($"RESPAWN IN " + e.timerValue);
        }

        private void PlayerStats_OnDeathed(object sender, EventArgs e) => Show();
      
        private void PlayersStats_OnRestored(object sender, EventArgs e) => Hide();
       
        private void Hide() => gameObject.SetActive(false);

        private void Show() => gameObject.SetActive(true);

    }
}
