using System;
using System.Collections.Generic;
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


        private void Update()
        {
            if (GameInput.Instance.GetSprintValue() == 1)
                DecreaseStamina();
            else
                IncreaseStamina();
        }


        private void IncreaseStamina()
        {
            if (Stamina == playerStatsSO.MaxStamina) return;

            if (Stamina < playerStatsSO.MaxStamina)
                Stamina += Time.deltaTime;
            else
                Stamina = playerStatsSO.MaxStamina;

            ChangeStaminaValue();

        }

        private void DecreaseStamina()
        {
            if (Stamina == 0) return;

            if (Stamina > 0)
                Stamina -= Time.deltaTime;
            else
                Stamina = 0;

            ChangeStaminaValue();
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

    }
}
