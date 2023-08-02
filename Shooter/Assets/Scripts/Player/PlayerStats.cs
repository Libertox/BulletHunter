﻿using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor.Rendering;
using UnityEngine;


namespace Shooter
{
    public class PlayerStats:NetworkBehaviour, IDamageable
    {
        public static event EventHandler OnAnyPlayerSpawn;

        public static PlayerStats Instance { get; private set; }

        public event EventHandler<OnStatsChangedEventArgs> OnStaminaChanged;
        public event EventHandler<OnStatsChangedEventArgs> OnHealthChanged;
        public event EventHandler<OnStatsChangedEventArgs> OnInvulnerabilityChanged;

        public event EventHandler<OnDeathedEventArgs> OnDeathed;
        public event EventHandler<OnWaitedEventArgs> OnRestoreWaited;
        public event EventHandler OnRestored;

        public class OnStatsChangedEventArgs: EventArgs { public float stats; }

        public class OnDeathedEventArgs : EventArgs { public ulong targetId; public ulong ownerId; }

        public class OnWaitedEventArgs : EventArgs { public float timerValue; }

        public float Health { get; private set; }
        public float Stamina { get; private set; }
   

        [SerializeField] private PlayerStatsSO playerStatsSO;

        private bool isInvulnerable;
        private ulong lastPlayerHitId;

        private void Start()
        {
            Health = playerStatsSO.MaxHealth;
            Stamina = playerStatsSO.MaxStamina;      
        }

        private void Update()
        {
            if (!IsOwner) return;

            if (Input.GetKeyDown(KeyCode.Z))
            {
                /*OnDeathed?.Invoke(this, EventArgs.Empty);
                StartCoroutine(Restore());
                GameManager.Instance.SetGameState(GameManager.GameState.Respawn);*/
                StartCoroutine(InvulnerabilityCountdown());
            }
                
        }

        public override void OnNetworkSpawn()
        {
            if(IsOwner)
            {
                Instance = this;
            }

            OnAnyPlayerSpawn?.Invoke(this, EventArgs.Empty);
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
            if (Health == playerStatsSO.MaxHealth) return;

            if (Health < playerStatsSO.MaxHealth)
                Health += increaseValue;
            else
                Health = playerStatsSO.MaxHealth;

            ChnageHealthValue();
        }

        public void DecreaseHealth(float decreaseValue)
        {
            if (Health <= 0) return;

            if (Health > 0)
            {
                Health -= decreaseValue;
                if (Health <= 0)
                {
                    OnDeathed?.Invoke(this, new OnDeathedEventArgs { targetId = lastPlayerHitId , ownerId = OwnerClientId});
                    StartCoroutine(Restore());
                }
            }    
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

        private IEnumerator Restore()
        {
            float restoreTimer = 10;
            WaitForSeconds waitForSeconds = new WaitForSeconds(1);
            OnRestoreWaited?.Invoke(this, new OnWaitedEventArgs { timerValue = restoreTimer });
            while (restoreTimer > 0)
            {
                yield return waitForSeconds;
                restoreTimer--;
                OnRestoreWaited?.Invoke(this, new OnWaitedEventArgs { timerValue = restoreTimer });
            }
            OnRestored?.Invoke(this, EventArgs.Empty);
            IncreaseHealth(playerStatsSO.MaxHealth);
            GameManager.Instance.SetGameState(GameManager.GameState.Play);
            transform.position = GameManager.Instance.GetRandomPosition();
            StartCoroutine(InvulnerabilityCountdown());
        }

        private IEnumerator InvulnerabilityCountdown()
        {
            float restoreTimer = 10;
            float timer = restoreTimer;
            isInvulnerable = true;
            OnInvulnerabilityChanged?.Invoke(this, new OnStatsChangedEventArgs { stats = timer / restoreTimer });
            while (timer > 0)
            {
                timer -= Time.deltaTime * 2;
                OnInvulnerabilityChanged?.Invoke(this, new OnStatsChangedEventArgs { stats = timer / restoreTimer });
                yield return new WaitForSeconds(Time.deltaTime);
                
            }
            isInvulnerable = false;
        }


        public void TakeDamage(float damage, ulong clientId)
        {
            if(!GameManager.Instance.IsStartState() && !isInvulnerable)
            {
                lastPlayerHitId = clientId;
                DecreaseHealth(damage);
            }
                
        }

        public NetworkObject GetNetworkObject()
        {
            return NetworkObject;
        }
    }
}
