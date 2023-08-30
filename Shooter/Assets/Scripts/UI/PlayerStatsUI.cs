using System;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHaunter.UI
{
    public class PlayerStatsUI: MonoBehaviour
    {
        [SerializeField] private BarUI healthBar;
        [SerializeField] private BarUI staminaBar;
        [SerializeField] private BarUI armorBar;
        [SerializeField] private BarUI invulnerabilityBar;

        private void Start()
        {
            if(PlayerStats.Instance != null)
            {
                PlayerStats.Instance.OnStaminaChanged += PlayerStats_OnStaminaChanged;
                PlayerStats.Instance.OnHealthChanged += PlayerStats_OnHealthChanged;
                PlayerStats.Instance.OnArmorChanged += PlayerStats_OnArmorChanged;
                PlayerStats.Instance.OnInvulnerabilityChanged += PlayerStats_OnInvulnerabilityChanged;
            }
            else
            {
                PlayerStats.OnAnyPlayerSpawn += PlayerStats_OnAnyPlayerSpawn;
            }
          
        }

        private void PlayerStats_OnAnyPlayerSpawn(object sender, EventArgs e)
        {
            if (PlayerStats.Instance != null)
            {
                PlayerStats.Instance.OnStaminaChanged -= PlayerStats_OnStaminaChanged;
                PlayerStats.Instance.OnStaminaChanged += PlayerStats_OnStaminaChanged;

                PlayerStats.Instance.OnHealthChanged -= PlayerStats_OnHealthChanged;
                PlayerStats.Instance.OnHealthChanged += PlayerStats_OnHealthChanged;

                PlayerStats.Instance.OnInvulnerabilityChanged -= PlayerStats_OnInvulnerabilityChanged;
                PlayerStats.Instance.OnInvulnerabilityChanged += PlayerStats_OnInvulnerabilityChanged;

                PlayerStats.Instance.OnArmorChanged -= PlayerStats_OnArmorChanged;
                PlayerStats.Instance.OnArmorChanged += PlayerStats_OnArmorChanged;
            }
        }

        private void PlayerStats_OnHealthChanged(object sender, PlayerStats.OnStatsChangedEventArgs e) => 
            healthBar.ChangeFillAmountSlowly(e.stats);
        

        private void PlayerStats_OnStaminaChanged(object sender, PlayerStats.OnStatsChangedEventArgs e) => 
            staminaBar.ChangeFillAmountImmediately(e.stats);
        
        private void PlayerStats_OnInvulnerabilityChanged(object sender, PlayerStats.OnStatsChangedEventArgs e) =>
            invulnerabilityBar.ChangeFillAmountImmediately(e.stats);
       

        private void PlayerStats_OnArmorChanged(object sender, PlayerStats.OnStatsChangedEventArgs e) => 
            armorBar.ChangeFillAmountSlowly(e.stats);
        
    }
}
