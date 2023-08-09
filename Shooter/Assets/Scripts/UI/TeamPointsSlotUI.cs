using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BulletHaunter.UI
{
    public class TeamPointsSlotUI:MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI teamNameText;
        [SerializeField] private TextMeshProUGUI pointsText;


        public void UpdateContent(string teamName, Color teamColor, int points)
        {
            teamNameText.SetText(teamName + " TEAM");
            teamNameText.color = teamColor;
            pointsText.SetText(points.ToString());
        }

        public void Show() => gameObject.SetActive(true);

        public void Hide() => gameObject.SetActive(false);

    }
}
