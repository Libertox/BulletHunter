using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace Shooter.UI
{
    public class TeamPointsTableUI:MonoBehaviour
    {
        [SerializeField] private TeamPointsSlotUI teamPointsSlotTemplate;
        [SerializeField] private Transform tableTitleTransform;
        [SerializeField] private RectTransform containrerRectTransform;


        private void Start()
        {
            if(GameInput.Instance != null)
            {
                GameInput.Instance.OnTableShowed += GameInput_OnTableShowed;
                GameInput.Instance.OnTableHided += GameInput_OnTableHided;
                GameInput.Instance.OnPaused += GameInput_OnPaused;
            }
            
            if(GameManager.Instance != null)
                GameManager.Instance.OnTeamPointsChanged += GameManager_OnTeamPointsChanged;

            UpdatePointsTable();
            Hide();
        }

        private void GameManager_OnTeamPointsChanged(object sender, EventArgs e)
        {
            UpdatePointsTable();
        }

        private void GameInput_OnPaused(object sender, EventArgs e)
        {
            Hide();
        }

        private void GameInput_OnTableHided(object sender, EventArgs e)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Hide();
        }

        private void GameInput_OnTableShowed(object sender, EventArgs e)
        {
            Cursor.lockState = CursorLockMode.None;
            Show();
            UpdatePointsTable();
        }

      

        private void UpdatePointsTable()
        {
            teamPointsSlotTemplate.Hide();

            foreach (Transform child in containrerRectTransform)
            {
                if (child == tableTitleTransform.transform || child == teamPointsSlotTemplate.transform) continue;

                Destroy(child.gameObject);
            }
            containrerRectTransform.sizeDelta = new Vector2(0, 200);

            var ordered = GameManager.Instance.TeamPointsDictionary.OrderByDescending(x => x.Value);

            foreach (var team in ordered)
            {
                TeamPointsSlotUI teamPointsSlotUI = Instantiate(teamPointsSlotTemplate, containrerRectTransform);
                teamPointsSlotUI.Show();
                teamPointsSlotUI.UpdateContent(GameManagerMultiplayer.Instance.GetTeamName(team.Key), GameManagerMultiplayer.Instance.GetTeamColor(team.Key), team.Value);
                containrerRectTransform.sizeDelta = new Vector2(0, containrerRectTransform.rect.height + 150);
            }
        }

        public void Show() => gameObject.SetActive(true);

        public void Hide() => gameObject.SetActive(false);


    }
}
