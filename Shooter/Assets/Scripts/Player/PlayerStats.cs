using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor.Rendering;
using UnityEngine;


namespace BulletHaunter
{
    public class PlayerStats:NetworkBehaviour, IDamageable
    {
        public static event EventHandler OnAnyPlayerSpawn;

        public static PlayerStats Instance { get; private set; }

        public event EventHandler<OnStatsChangedEventArgs> OnStaminaChanged;
        public event EventHandler<OnStatsChangedEventArgs> OnHealthChanged;
        public event EventHandler<OnStatsChangedEventArgs> OnInvulnerabilityChanged;
        public event EventHandler<OnStatsChangedEventArgs> OnArmorChanged;

        public event EventHandler<OnDeathedEventArgs> OnDeathed;
        public event EventHandler<OnWaitedEventArgs> OnRestoreWaited;
        public event EventHandler OnRestored;

        public class OnStatsChangedEventArgs: EventArgs { public float stats; }
        public class OnDeathedEventArgs : EventArgs { public ulong targetId; public ulong ownerId; }
        public class OnWaitedEventArgs : EventArgs { public float timerValue; }

        public float Stamina { get; private set; }

        [SerializeField] private PlayerStatsSO playerStatsSO;

        private float health;
        private float armor;
        

        private bool isInvulnerable;
        private bool haveStaminaBust;

        private ulong lastPlayerHitId;

        private void Start()
        {
            health = playerStatsSO.MaxHealth;
            Stamina = playerStatsSO.MaxStamina;
        }

        private void Update()
        {
            if (!IsOwner) return;

            if (Input.GetKeyDown(KeyCode.Z))
            {
                /*OnDeathed?.Invoke(this, new OnDeathedEventArgs());
                StartCoroutine(Restore());
                GameManager.Instance.SetGameState(GameManager.GameState.Respawn);*/
                // StartCoroutine(InvulnerabilityCountdown());
                IncreaseHealth(5f);
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                //transform.position = GameManager.Instance.GetRandomPosition();
            }
                
        }

        public override void OnNetworkSpawn()
        {
            if(IsOwner)
                Instance = this;
 
            OnAnyPlayerSpawn?.Invoke(this, EventArgs.Empty);
        }

        public void IncreaseStamina(float increaseValue)
        {
            Stamina += increaseValue;

            if (Stamina > playerStatsSO.MaxStamina)
                Stamina = playerStatsSO.MaxStamina;

            ChangeStaminaValue();
        }

        public void DecreaseStamina(float decreaseValue)
        {
            if (haveStaminaBust) return;

            Stamina -= decreaseValue;

            if (Stamina < 0)
                Stamina = 0;

            ChangeStaminaValue();
        }

        public void IncreaseHealth(float increaseValue)
        {
            health += increaseValue;

            if (health > playerStatsSO.MaxHealth)
                health = playerStatsSO.MaxHealth;

            ChnageHealthValue();
        }

        public bool DecreaseHealth(float decreaseValue)
        {
            if (health > 0)
            {
                health -= decreaseValue;
                ChnageHealthValue();
                if (health <= 0)
                {
                    OnDeathed?.Invoke(this, new OnDeathedEventArgs { targetId = lastPlayerHitId , ownerId = OwnerClientId});
                    StartCoroutine(RestoreCoroutine());
                    health = 0;
                    return true;
                }
            }
  
            return false;        
        }

        public void IncreaseArmor(float increaseValue)
        {
            armor += increaseValue;

            if (armor > playerStatsSO.MaxArmor)
                armor = playerStatsSO.MaxArmor;

            ChangeArmorValue();
        }

        public void DecreaseArmor(float decreaseValue)
        {
            armor -= decreaseValue;

            if (armor < 0)        
                armor = 0;

            ChangeArmorValue();
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
                stats = health / playerStatsSO.MaxHealth
            });
        }

        private void ChangeArmorValue()
        {
            OnArmorChanged?.Invoke(this, new OnStatsChangedEventArgs
            {
                stats = armor / playerStatsSO.MaxArmor
            });
        }

        private IEnumerator RestoreCoroutine()
        {
            float restoreTimer = playerStatsSO.RestoreCooldown;
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
            StartCoroutine(InvulnerabilityCountdownCoroutine());
        }

        private IEnumerator InvulnerabilityCountdownCoroutine()
        {
            float InvulnerabilityTimer = playerStatsSO.InvulnerabilityCooldown;
            isInvulnerable = true;
            OnInvulnerabilityChanged?.Invoke(this, new OnStatsChangedEventArgs { stats = InvulnerabilityTimer / playerStatsSO.InvulnerabilityCooldown });
            while (InvulnerabilityTimer > 0)
            {
                InvulnerabilityTimer -= Time.deltaTime * 2;
                OnInvulnerabilityChanged?.Invoke(this, new OnStatsChangedEventArgs { stats = InvulnerabilityTimer / playerStatsSO.InvulnerabilityCooldown });
                yield return new WaitForSeconds(Time.deltaTime);
                
            }
            isInvulnerable = false;
        }

        public void SetStaminaBust(float duration) => StartCoroutine(StaminaBustCoroutine(duration));

        private IEnumerator StaminaBustCoroutine(float duration)
        {
            haveStaminaBust = true;
            yield return new WaitForSeconds(duration);
            haveStaminaBust = false;
        }


        public bool TakeDamage(float damage, ulong clientId)
        {
            if(!GameManager.Instance.IsStartState() && !isInvulnerable)
            {
                lastPlayerHitId = clientId;
                SoundManager.Instance.PlayPlayerTakeDamageSound(transform.position);
                if (armor <= 0)
                     return DecreaseHealth(damage);
                else
                    DecreaseArmor(damage);
            }
            return false;    
        }

        public NetworkObject GetNetworkObject() => NetworkObject;

        public static void ResetStaticData()
        {
            OnAnyPlayerSpawn = null;
        }
       
    }
}
