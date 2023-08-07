using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Shooter.UI
{
    public class WinSceneUI:MonoBehaviour
    {
        [SerializeField] private Button backToMainMenuButton;
        [SerializeField] private Button pointsTableButton;
        [SerializeField] private TextMeshProUGUI teamWonText;

        [SerializeField] private TeamPointsTableUI teamPointsTable;

        private void Awake()
        {
            backToMainMenuButton.onClick.AddListener(() => SceneLoader.Load(SceneLoader.GameScene.MainMenu));

            pointsTableButton.onClick.AddListener(() =>
            {
                if (teamPointsTable.isActiveAndEnabled)
                    teamPointsTable.Hide();
                else
                    teamPointsTable.Show();
            });
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.None;
            teamWonText.color = GameManagerMultiplayer.Instance.GetTeamColor(GameManagerMultiplayer.Instance.WinningTeam);
            teamWonText.SetText(GameManagerMultiplayer.Instance.GetTeamName(GameManagerMultiplayer.Instance.WinningTeam) + " TEAM WON");
        }
    }
}
