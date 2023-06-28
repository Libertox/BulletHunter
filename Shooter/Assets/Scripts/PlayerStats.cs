using System;
using System.Collections.Generic;
using UnityEngine;


namespace Shooter
{
    public class PlayerStats:MonoBehaviour
    {
        public float Health { get; private set; }
        public float Stamina { get; private set; }
        public int GunAmmo { get; private set; }
        public int RifleAmmo { get; private set; }
        public int GrenadeAmount { get; private set; }

        [SerializeField] private PlayerStatsSO playerStatsSO;


        private void Start()
        {
            Health = playerStatsSO.MaxHealth;
            Stamina = playerStatsSO.MaxStamina;
        }

        private void Update()
        {
            if (GameInput.Instance.GetSprintValue() != 0)
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
        }

        private void DecreaseStamina()
        {
            if (Stamina == 0) return;

            if (Stamina > 0)
                Stamina -= Time.deltaTime;
            else
                Stamina = 0;
        }

    }
}
