using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;

namespace BulletHaunter
{
    public class KillMessageUI:MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI killMessageText;
        [SerializeField] private CanvasGroup canvasGroup;
        private bool show;
        private float displayTimer;
        private readonly float cooldownShowText = 5f;

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

        private void OnDestroy()
        {
            PlayerShoot.OnAnyPlayerKilled -= PlayerShoot_OnAnyPlayerKilled;
        }

        private void PlayerShoot_OnAnyPlayerKilled(object sender, PlayerShoot.OnAnyPlayerKilledEventArgs e)
        {
            SetMessageText("YOU KILLED ", e.targetId);
        }

        private void PlayerStats_OnDeathed(object sender, PlayerStats.OnDeathedEventArgs e)
        {
            SetMessageText("KILLED BY ", e.targetId);
        }

        private void SetMessageText(string message, ulong clientId)
        {
            Show();
            show = true;
            PlayerData playerData = GameManagerMultiplayer.Instance.GetPlayerDataFromClientId(clientId);
            killMessageText.color = GameManagerMultiplayer.Instance.GetTeamColor(playerData.teamColorId);
            killMessageText.SetText(message + playerData.playerName.ToString());
        }

        private void Show() => gameObject.SetActive(true);

        private void Hide() => gameObject.SetActive(false);

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

    }
}
