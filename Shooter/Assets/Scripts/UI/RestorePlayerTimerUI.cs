using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Shooter
{
    public class RestorePlayerTimerUI:MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI restoreTimerText;

        private void Start()
        {
            if(PlayerStats.Instnace != null)
            {
                PlayerStats.Instnace.OnDeathed += PlayerStats_OnDeathed;
                PlayerStats.Instnace.OnRestored += PlayersStats_OnRestored;
                PlayerStats.Instnace.OnRestoreWaited += PlayerStats_OnRestoreWaited;
            }
            else
            {
                PlayerStats.OnAnyPlayerSpawn += PlayerStats_OnAnyPlayerSpawn;
            }

            Hide();
        }

     
        private void PlayerStats_OnAnyPlayerSpawn(object sender, EventArgs e)
        {
            if (PlayerStats.Instnace != null)
            {
                PlayerStats.Instnace.OnDeathed -= PlayerStats_OnDeathed;
                PlayerStats.Instnace.OnDeathed += PlayerStats_OnDeathed;

                PlayerStats.Instnace.OnRestoreWaited -= PlayerStats_OnRestoreWaited;
                PlayerStats.Instnace.OnRestoreWaited += PlayerStats_OnRestoreWaited;

                PlayerStats.Instnace.OnRestored -= PlayersStats_OnRestored;
                PlayerStats.Instnace.OnRestored += PlayersStats_OnRestored;
            }
        }

        private void PlayerStats_OnRestoreWaited(object sender, PlayerStats.OnRestoreWaitedEventArgs e)
        {
            restoreTimerText.SetText($"RESPAWN IN " + e.timerValue);
        }

        private void PlayerStats_OnDeathed(object sender, EventArgs e)
        {
            Show();
        }

        private void PlayersStats_OnRestored(object sender, EventArgs e)
        {
            Hide();
        }

        private void Hide() => gameObject.SetActive(false);

        private void Show() => gameObject.SetActive(true);

    }
}
