using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BulletHaunter.UI
{
    public class WinSceneUI:MonoBehaviour
    {
        [SerializeField] private Button backToMainMenuButton;
        [SerializeField] private Button pointsTableButton;
        [SerializeField] private TextMeshProUGUI teamWonText;

        [SerializeField] private TeamPointsTableUI teamPointsTable;

        private void Awake()
        {
            backToMainMenuButton.onClick.AddListener(() => 
            {
                SceneLoader.Load(SceneLoader.GameScene.MainMenu);
                SoundManager.Instance.PlayButtonSound();
            });

            pointsTableButton.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlayButtonSound();
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
