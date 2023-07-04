using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shooter.UI
{
    public class PlayerStatsUI: MonoBehaviour
    {
        [SerializeField] private BarUI healthBar;
        [SerializeField] private BarUI staminaBar;

        private void Start()
        {
            PlayerStats.OnStaminaChanged += PlayerStats_OnStaminaChanged;
            PlayerStats.OnHealthChanged += PlayerStats_OnHealthChanged;
        }

        private void PlayerStats_OnHealthChanged(object sender, PlayerStats.OnStatsChangedEventArgs e)
        {
            healthBar.ChangeFillAmount(e.stats);
        }

        private void PlayerStats_OnStaminaChanged(object sender, PlayerStats.OnStatsChangedEventArgs e)
        {
            staminaBar.ChangeFillAmount(e.stats);
        }
    }
}
