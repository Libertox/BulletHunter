using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Shooter
{
    public class Inventory : MonoBehaviour
    {
        private const int maxGunMagazine = 4;
        private const int maxShoutgunMagazine = 2;
        private const int maxRifleMagazine = 6;
        private const int maxSniperMagazine = 3;
        private const int maxGrenadeMagazine = 5;

        public static Inventory Instance { get; private set; }

        public event EventHandler OnAmmoChanged;
        public event EventHandler<OnReloadedEventArgs> OnReloaded;
        public event EventHandler OnCanelReloaded;
        public event EventHandler<OnGrenadeAmountChangedEventArgs> OnGrenadeAdded;
        public event EventHandler<OnGrenadeAmountChangedEventArgs> OnGrenadeSubstracted;

        public event EventHandler<OnSelectedWeaponChangedEventArgs> OnSelectedWeaponChanged; 
        public event EventHandler<OnSelectedWeaponChangedEventArgs> OnSelectedWeaponDroped;
        public class OnSelectedWeaponChangedEventArgs : EventArgs { public WeaponInstance selectedWeapon; }
        public class OnGrenadeAmountChangedEventArgs: EventArgs { public int grenadeAmount; }

        public class OnReloadedEventArgs: EventArgs { public float reloadTime; }

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

        private bool isReload;
        private float time;

        private void Awake()
        {
            if (!Instance)
                Instance = this;
            else
                Destroy(gameObject);

            ownedWeapon = new WeaponInstance[MaxNumberWeapon];
         
        }
        private void Start()
        {
            GameInput.Instance.OnWeaponSelected += GameInput_OnWeaponSelected;
            GameInput.Instance.OnWeaponDroped += GameInput_OnWeaponDroped;
            GameInput.Instance.OnReloaded += GameInput_OnReloaded;
        }

        private void GameInput_OnReloaded(object sender, EventArgs e) 
        { 
            if(UseWeapon != null)
                isReload = true; 
        } 
       
        private void GameInput_OnWeaponDroped(object sender, EventArgs e) => DropUseWeapon();
     
        private void GameInput_OnWeaponSelected(object sender, GameInput.OnWeaponSelectedEventArgs e)
        {
            UseWeapon = ownedWeapon[e.selectWeaponIndex];
            useWeaponIndex = e.selectWeaponIndex;
            CancelReload();
            OnSelectedWeaponChanged?.Invoke(this, new OnSelectedWeaponChangedEventArgs
            {
                selectedWeapon = UseWeapon
            });
        }

        private void Update()
        {
            if (!CanReload()) return;

            time += Time.deltaTime;
            OnReloaded?.Invoke(this, new OnReloadedEventArgs
            {
                reloadTime = time / UseWeapon.WeaponSO.ReloadTime
            });
            if(time > UseWeapon.WeaponSO.ReloadTime)
            {
                Reload();
                CancelReload();
            }  
        }

        public bool AddWeapon(WeaponInstance weaponToAdd)
        {
            for (int i = 0; i < ownedWeapon.Length; i++)
            {
                if (ownedWeapon[i] == null)
                {
                    ownedWeapon[i] = weaponToAdd;

                    if (UseWeapon  == null)
                    {
                        UseWeapon = ownedWeapon[useWeaponIndex];
                        OnSelectedWeaponChanged?.Invoke(this, new OnSelectedWeaponChangedEventArgs
                        {
                            selectedWeapon = UseWeapon
                        });
                    }

                    AddMagazine(weaponToAdd.WeaponSO.WeaponType,weaponToAdd.MagazineAmount);
                    return true;
                }               
            }
            return false;
        }

        public int GetUseMagazine() => MagazineAmount[UseWeapon.WeaponSO.WeaponType];

        public void AddMagazine(WeaponType typeOfWeapon , int magazineNumber)
        {
            if (MagazineAmount[typeOfWeapon] + magazineNumber < maxAmmoMagazine[typeOfWeapon])
                MagazineAmount[typeOfWeapon] += magazineNumber;
            else
                MagazineAmount[typeOfWeapon] = maxAmmoMagazine[typeOfWeapon];

            OnAmmoChanged?.Invoke(this, EventArgs.Empty);
        }

        public void AmmoChanged()
        {
            OnAmmoChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool AddOneGranade()
        {
            if(GrenadeAmount < maxGrenadeMagazine)
            {
                OnGrenadeAdded?.Invoke(this, new OnGrenadeAmountChangedEventArgs
                {
                    grenadeAmount = GrenadeAmount
                });
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
                OnGrenadeSubstracted?.Invoke(this, new OnGrenadeAmountChangedEventArgs
                {
                    grenadeAmount = GrenadeAmount
                });

            }            
        }

        private void DropUseWeapon()
        {
            if (UseWeapon == null) return;

            OnSelectedWeaponDroped?.Invoke(this, new OnSelectedWeaponChangedEventArgs
            {
                selectedWeapon = UseWeapon
            });
            CancelReload();
            UseWeapon = null;
            ownedWeapon[useWeaponIndex] = null;
            
        }

        public bool CanShoot()
        {
            if (UseWeapon == null) return false;

            if (UseWeapon.AmmoAmount <= 0)
            {
                isReload = true;
                return false;
            }

            return true;
        }

        public bool CanThrowGrenade() => GrenadeAmount > 0;

        private bool CanReload() => isReload && UseWeapon != null && GetUseMagazine() > 0;

        private void Reload()
        {
           MagazineAmount[UseWeapon.WeaponSO.WeaponType]--;
           UseWeapon.FillAmmo();
            
           OnAmmoChanged?.Invoke(this, EventArgs.Empty);
        }

        private void CancelReload()
        {
            isReload = false;
            time = 0;
            OnCanelReloaded?.Invoke(this, EventArgs.Empty);
        }

    }
}
