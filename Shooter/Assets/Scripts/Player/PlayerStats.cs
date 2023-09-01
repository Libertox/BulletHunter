using System;
using System.Collections;
using Unity.Netcode;
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
        public event EventHandler<OnAnyPlayerKilledEventArgs> OnAnyPlayerKilled;
        public class OnAnyPlayerKilledEventArgs : EventArgs { public ulong targetId; }

        public class OnStatsChangedEventArgs: EventArgs { public float stats; }
        public class OnDeathedEventArgs : EventArgs { public ulong targetId; public ulong ownerId; }
        public class OnWaitedEventArgs : EventArgs { public float timerValue; }

        public float Stamina { get; private set; }

        [SerializeField] private PlayerStatsSO playerStatsSO;

        private float health;
        private float armor;
        
        private bool isInvulnerable;
        private bool haveStaminaBust;

        private ulong opponentHitId;

        private void Start()
        {
            health = playerStatsSO.MaxHealth;
            Stamina = playerStatsSO.MaxStamina;
        }
 
        public override void OnNetworkSpawn()
        {
            if(IsOwner)
                Instance = this;

            StartCoroutine(InvulnerabilityCountdownCoroutine(GameManager.Instance.StartGameTimer.Value));
            OnAnyPlayerSpawn?.Invoke(this, EventArgs.Empty);
        }

        private IEnumerator InvulnerabilityCountdownCoroutine(float invulnerabilityCooldown)
        {
            float InvulnerabilityTimer = invulnerabilityCooldown;
            isInvulnerable = true;
            OnInvulnerabilityChanged?.Invoke(this, new OnStatsChangedEventArgs { stats = InvulnerabilityTimer / invulnerabilityCooldown });
            while (InvulnerabilityTimer > 0)
            {
                InvulnerabilityTimer -= Time.deltaTime;
                OnInvulnerabilityChanged?.Invoke(this, new OnStatsChangedEventArgs { stats = InvulnerabilityTimer / invulnerabilityCooldown });
                yield return new WaitForEndOfFrame();

            }
            isInvulnerable = false;
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
        private void ChangeStaminaValue()
        {
            OnStaminaChanged?.Invoke(this, new OnStatsChangedEventArgs
            {
                stats = Stamina / playerStatsSO.MaxStamina
            });
        }


        public void IncreaseHealth(float increaseValue)
        {
            health += increaseValue;

            if (health > playerStatsSO.MaxHealth)
                health = playerStatsSO.MaxHealth;

            ChnageHealthValue();
        }

        public void DecreaseHealth(float decreaseValue)
        {
            if (health > 0)
            {
                health -= decreaseValue;
                
                if (health <= 0)
                {
                    OnDeathed?.Invoke(this, new OnDeathedEventArgs { targetId = opponentHitId , ownerId = OwnerClientId});
                    SendDeathMessageServerRpc(opponentHitId);
                    StartCoroutine(RestoreCoroutine());
                    health = 0;
                }

                ChnageHealthValue();
            }
     
        }

        [ServerRpc(RequireOwnership = false)]
        private void SendDeathMessageServerRpc(ulong clientId) => SendDeathMessageClientRpc(clientId);
         
        [ClientRpc()]
        private void SendDeathMessageClientRpc(ulong clientId)
        {
            if (NetworkManager.Singleton.LocalClientId == clientId)
                Instance.OnAnyPlayerKilled?.Invoke(this, new OnAnyPlayerKilledEventArgs { targetId = OwnerClientId});              
        }

        private void ChnageHealthValue()
        {
            OnHealthChanged?.Invoke(this, new OnStatsChangedEventArgs
            {
                stats = health / playerStatsSO.MaxHealth
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

            StartCoroutine(InvulnerabilityCountdownCoroutine(playerStatsSO.InvulnerabilityCooldown));
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

        private void ChangeArmorValue()
        {
            OnArmorChanged?.Invoke(this, new OnStatsChangedEventArgs
            {
                stats = armor / playerStatsSO.MaxArmor
            });
        }

        public void SetStaminaBust(float duration) => StartCoroutine(StaminaBustCoroutine(duration));

        private IEnumerator StaminaBustCoroutine(float duration)
        {
            haveStaminaBust = true;
            yield return new WaitForSeconds(duration);
            haveStaminaBust = false;
        }


        public void TakeDamage(float damage, ulong clientId)
        {
            if (CanTakeDamage())
            {
                opponentHitId = clientId;
                SoundManager.Instance.PlayPlayerTakeDamageSound(transform.position);

                if (armor <= 0)
                    DecreaseHealth(damage);
                else
                    DecreaseArmor(damage);
            }
        }

        private bool CanTakeDamage() => !isInvulnerable && IsOwner;
       
        public NetworkObject GetNetworkObject() => NetworkObject;

        public static void ResetStaticData() => OnAnyPlayerSpawn = null;     
    }
}
