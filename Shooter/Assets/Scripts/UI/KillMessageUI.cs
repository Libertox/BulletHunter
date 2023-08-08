using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;

namespace Shooter
{
    public class KillMessageUI:MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI killMessageText;
        [SerializeField] private CanvasGroup canvasGroup;
        private bool show;
        private float displayTimer;
        private float cooldownShowText = 5f;

        private void Start()
        {
            if(PlayerStats.Instance != null)
            {
                PlayerStats.Instance.OnDeathed += PlayerStats_OnDeathed;
            }
            else
            {
                PlayerStats.OnAnyPlayerSpawn += PlayerStats_OnAnyPlayerSpawn;
            }
            

            PlayerShoot.OnAnyPlayerKilled += PlayerShoot_OnAnyPlayerKilled;
 
            Hide();
        }

        private void PlayerStats_OnAnyPlayerSpawn(object sender, EventArgs e)
        {
            if (PlayerStats.Instance != null)
            {
                PlayerStats.Instance.OnDeathed -= PlayerStats_OnDeathed;
                PlayerStats.Instance.OnDeathed += PlayerStats_OnDeathed;
            }
        }

        private void PlayerShoot_OnAnyPlayerKilled(object sender, PlayerShoot.OnAnyPlayerKilledEventArgs e)
        {
            Show();
            show = true;
            PlayerData playerData = GameManagerMultiplayer.Instance.GetPlayerDataFromClientId(e.targetId);
            killMessageText.color = GameManagerMultiplayer.Instance.GetTeamColor(playerData.teamColorId);
            killMessageText.SetText("YOU KILLED " + playerData.playerName.ToString());
        }

        private void PlayerStats_OnDeathed(object sender, PlayerStats.OnDeathedEventArgs e)
        {
            Show();
            show = true;
            PlayerData playerData = GameManagerMultiplayer.Instance.GetPlayerDataFromClientId(e.targetId);
            killMessageText.color = GameManagerMultiplayer.Instance.GetTeamColor(playerData.teamColorId);
            killMessageText.SetText("KILLED BY " + playerData.playerName.ToString());
        }



        private void Show()
        {
            gameObject.SetActive(true);
        }

        private void Update()
        {
            if (show)
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 1f, Time.deltaTime);
                if (canvasGroup.alpha == 1f)
                {
                    displayTimer += Time.deltaTime;
                    if (displayTimer > cooldownShowText)
                        show = false;
                }
            }
            else
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 0f, Time.deltaTime);
                if (canvasGroup.alpha == 0f)
                    Hide();
            }

        }

        private void Hide() => gameObject.SetActive(false);

       


    }
}
