using System;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;


namespace Shooter
{
    public class PlayerStats:MonoBehaviour
    {
        public static event EventHandler<OnStatsChangedEventArgs> OnStaminaChanged;
        public static event EventHandler<OnStatsChangedEventArgs> OnHealthChanged;

        public class OnStatsChangedEventArgs: EventArgs { public float stats; }

        public float Health { get; private set; }
        public float Stamina { get; private set; }
   

        [SerializeField] private PlayerStatsSO playerStatsSO;

        private void Start()
        {
            Health = playerStatsSO.MaxHealth;
            Stamina = playerStatsSO.MaxStamina;
        }

        public void IncreaseStamina(float increaseValue)
        {
            if (Stamina == playerStatsSO.MaxStamina) return;

            if (Stamina < playerStatsSO.MaxStamina)
                Stamina += increaseValue;
            else
                Stamina = playerStatsSO.MaxStamina;

            ChangeStaminaValue();

        }

        public void DecreaseStamina(float decreaseValue)
        {
            if (Stamina == 0) return;

            if (Stamina > 0)
                Stamina -= decreaseValue;
            else
                Stamina = 0;

            ChangeStaminaValue();
        }

        public void IncreaseHealth(float increaseValue)
        {
            if (Stamina == playerStatsSO.MaxHealth) return;

            if (Stamina < playerStatsSO.MaxHealth)
                Health += increaseValue;
            else
                Health = playerStatsSO.MaxHealth;

            ChnageHealthValue();
        }

        public void DecreaseHealth(float decreaseValue)
        {
            if (Health == 0) return;

            if (Health > 0)
                Health -= decreaseValue;
            else
                Health = 0;

            ChnageHealthValue();
        }

        private void ChangeStaminaValue()
        {
            OnStaminaChanged?.Invoke(this, new OnStatsChangedEventArgs
            {
                stats = Stamina / playerStatsSO.MaxStamina
            });
        }

        private void ChnageHealthValue()
        {
            OnHealthChanged?.Invoke(this, new OnStatsChangedEventArgs
            {
                stats = Health / playerStatsSO.MaxHealth
            });
        }

        public static void ResetStaticData()
        {
            OnHealthChanged = null;
            OnStaminaChanged = null;
        }

    }
}
