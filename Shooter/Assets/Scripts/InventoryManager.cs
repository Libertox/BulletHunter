using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Unity.Netcode;

namespace Shooter
{
    public class InventoryManager : NetworkBehaviour
    {
        private const int maxGunMagazine = 4;
        private const int maxShoutgunMagazine = 2;
        private const int maxRifleMagazine = 6;
        private const int maxSniperMagazine = 3;
        private const int maxGrenadeMagazine = 5;

        public static InventoryManager Instance { get; private set; }

        public event EventHandler OnAmmoChanged;
        public event EventHandler<OnGrenadeAmountChangedEventArgs> OnGrenadeAdded;
        public event EventHandler<OnGrenadeAmountChangedEventArgs> OnGrenadeSubstracted;

        public event EventHandler<OnSelectedWeaponChangedEventArgs> OnSelectedWeaponChanged;
        public event EventHandler<OnSelectedWeaponChangedEventArgs> OnSelectedWeaponDroped;
        public class OnSelectedWeaponChangedEventArgs : EventArgs { public WeaponInstance selectedWeapon; }
        public class OnGrenadeAmountChangedEventArgs : EventArgs { public int grenadeAmount; }

        public static int MaxNumberWeapon { get; private set; } = 4;
        public WeaponInstance UseWeapon { get; private set; }

        private WeaponInstance[] ownedWeapon;
        private int useWeaponIndex;

        public int GrenadeAmount { get; private set; }

        public Dictionary<WeaponType, int> MagazineAmount { get; private set; } = new Dictionary<WeaponType, int>
        {
            {WeaponType.Gun,0 },
            {WeaponType.Rifle,0 },
            {WeaponType.Sniper,0 },
            {WeaponType.Shoutgun,0 },
        };


        private Dictionary<WeaponType, int> maxAmmoMagazine = new Dictionary<WeaponType, int>
        {
                {WeaponType.Gun,maxGunMagazine },
                {WeaponType.Rifle,maxRifleMagazine},
                {WeaponType.Shoutgun,maxShoutgunMagazine},
                {WeaponType.Sniper,maxSniperMagazine },
        };


        private void Awake()
        {
            Instance = this;

            ownedWeapon = new WeaponInstance[MaxNumberWeapon];

        }
        private void Start()
        {
            GameInput.Instance.OnWeaponSelected += GameInput_OnWeaponSelected;
            GameInput.Instance.OnWeaponDroped += GameInput_OnWeaponDroped;

            if(PlayerStats.Instance != null)
            {
                PlayerStats.Instance.OnRestored += PlayerStats_OnRestored;
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
                PlayerStats.Instance.OnRestored -= PlayerStats_OnRestored;
                PlayerStats.Instance.OnRestored += PlayerStats_OnRestored;
            }
        }

        private void PlayerStats_OnRestored(object sender, EventArgs e)
        {
            ownedWeapon = new WeaponInstance[MaxNumberWeapon];
            UseWeapon = null;
            MagazineAmount = new Dictionary<WeaponType, int>
            {
                {WeaponType.Gun,0 },
                {WeaponType.Rifle,0 },
                {WeaponType.Sniper,0 },
                {WeaponType.Shoutgun,0 },
            };
            OnSelectedWeaponChanged?.Invoke(this, new OnSelectedWeaponChangedEventArgs { selectedWeapon = UseWeapon });
            OnAmmoChanged?.Invoke(this, EventArgs.Empty);
            while(GrenadeAmount != 0)
            {
                GrenadeAmount--;
                OnGrenadeSubstracted?.Invoke(this, new OnGrenadeAmountChangedEventArgs { grenadeAmount = GrenadeAmount });
            }
            
        }

        private void GameInput_OnWeaponDroped(object sender, EventArgs e) => DropUseWeapon();

        private void GameInput_OnWeaponSelected(object sender, GameInput.OnWeaponSelectedEventArgs e)
        {
            UseWeapon = ownedWeapon[e.selectWeaponIndex];
            useWeaponIndex = e.selectWeaponIndex;
            OnSelectedWeaponChanged?.Invoke(this, new OnSelectedWeaponChangedEventArgs { selectedWeapon = UseWeapon });
        }

        public bool AddWeapon(WeaponInstance weaponToAdd)
        {
            for (int i = 0; i < ownedWeapon.Length; i++)
            {
                if (ownedWeapon[i] == null)
                {
                    ownedWeapon[i] = weaponToAdd;

                    if (UseWeapon == null)
                    {
                        UseWeapon = ownedWeapon[useWeaponIndex];
                        OnSelectedWeaponChanged?.Invoke(this, new OnSelectedWeaponChangedEventArgs { selectedWeapon = UseWeapon });
                    }

                    AddMagazine(weaponToAdd.WeaponSO.WeaponType, weaponToAdd.MagazineAmount);
                    return true;
                }
            }
            return false;
        }

        public int GetUseMagazine() => MagazineAmount[UseWeapon.WeaponSO.WeaponType];

        public void AddMagazine(WeaponType typeOfWeapon, int magazineNumber)
        {
            if (MagazineAmount[typeOfWeapon] + magazineNumber < maxAmmoMagazine[typeOfWeapon])
                MagazineAmount[typeOfWeapon] += magazineNumber;
            else
                MagazineAmount[typeOfWeapon] = maxAmmoMagazine[typeOfWeapon];

            OnAmmoChanged?.Invoke(this, EventArgs.Empty);
        }

        public void AmmoChanged() => OnAmmoChanged?.Invoke(this, EventArgs.Empty);

        public bool AddOneGranade()
        {
            if (GrenadeAmount < maxGrenadeMagazine)
            {
                OnGrenadeAdded?.Invoke(this, new OnGrenadeAmountChangedEventArgs { grenadeAmount = GrenadeAmount });

                GrenadeAmount++;
                return true;
            }
            return false;
        }

        public void SubstractGranade()
        {
            if (GrenadeAmount > 0)
            {
                GrenadeAmount--;
                OnGrenadeSubstracted?.Invoke(this, new OnGrenadeAmountChangedEventArgs { grenadeAmount = GrenadeAmount });
            }
        }

        private void DropUseWeapon()
        {
            if (UseWeapon == null) return;

            OnSelectedWeaponDroped?.Invoke(this, new OnSelectedWeaponChangedEventArgs { selectedWeapon = UseWeapon });

            UseWeapon = null;
            ownedWeapon[useWeaponIndex] = null;

        }

        public bool CanShoot()
        {
            if (UseWeapon == null || UseWeapon.AmmoAmount <= 0) return false;

            return true;
        }

        public bool CanThrowGrenade() => GrenadeAmount > 0;

        public void Reload()
        {
            MagazineAmount[UseWeapon.WeaponSO.WeaponType]--;
            UseWeapon.FillAmmo();

            OnAmmoChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
