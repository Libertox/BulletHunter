using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Shooter
{
    public class Inventory : MonoBehaviour
    {
        private const int maxGunMagazine = 64;
        private const int maxShoutgunMagazine = 48;
        private const int maxRifleMagazine = 192;
        private const int maxSniperMagazine = 32;
        private const int maxGranadeMagazine = 5;

        public static Inventory Instance { get; private set; }

        public event EventHandler OnAmmoChanged;
        public event EventHandler<OnReloadedEventArgs> OnReloaded;
        public event EventHandler OnCanelReloaded;
        public event EventHandler<OnSelectedWeaponChangedEventArgs> OnSelectedWeaponChanged; 
        public event EventHandler<OnSelectedWeaponChangedEventArgs> OnSelectedWeaponDroped;
        public class OnSelectedWeaponChangedEventArgs : EventArgs { public WeaponSO selectedWeapon; }

        public class OnReloadedEventArgs: EventArgs { public float reloadTime; }

        public static int MaxNumberWeapon { get; private set; } = 4;
        public WeaponSO UseWeapon { get; private set; }

        private WeaponSO[] ownedWeapon;
        private int useWeaponIndex;

        public Dictionary<WeaponType, int> AmmoAmount { get; private set; } = new Dictionary<WeaponType, int>
        {
                {WeaponType.Gun,0 },
                {WeaponType.Rifle,0 },
                {WeaponType.Sniper,0 },
                {WeaponType.Shoutgun,0 },
                {WeaponType.Grenade,0 }
        };

        public Dictionary<WeaponType, int> MagazineAmount { get; private set; } = new Dictionary<WeaponType, int>
        {
            {WeaponType.Gun,0 },
            {WeaponType.Rifle,0 },
            {WeaponType.Sniper,0 },
            {WeaponType.Shoutgun,0 },
            {WeaponType.Grenade,0 }
        };

        private Dictionary<WeaponType, int> maxAmmoMagazine = new Dictionary<WeaponType, int> 
        {
                {WeaponType.Gun,maxGunMagazine },
                {WeaponType.Rifle,maxRifleMagazine},
                {WeaponType.Shoutgun,maxShoutgunMagazine},
                {WeaponType.Sniper,maxSniperMagazine },
                {WeaponType.Grenade,maxGranadeMagazine }
        };

        private bool isReload;
        private float time;

        private void Awake()
        {
            if (!Instance)
                Instance = this;
            else
                Destroy(gameObject);

            ownedWeapon = new WeaponSO[MaxNumberWeapon];
         
        }
        private void Start()
        {
            GameInput.Instance.OnWeaponSelected += GameInput_OnWeaponSelected;
            GameInput.Instance.OnWeaponDroped += GameInput_OnWeaponDroped;
            GameInput.Instance.OnReloaded += GameInput_OnReloaded;
        }

        private void GameInput_OnReloaded(object sender, EventArgs e) => isReload = true;
       
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
                reloadTime = time / UseWeapon.ReloadTime
            });
            if(time > UseWeapon.ReloadTime)
            {
                Reload();
                CancelReload();
            }  

        }

        public bool AddWeapon(Weapon weaponToAdd)
        {
            for (int i = 0; i < ownedWeapon.Length; i++)
            {
                if (!ownedWeapon[i])
                {
                    ownedWeapon[i] = weaponToAdd.WeaponSO;

                    if (!UseWeapon)
                    {
                        UseWeapon = ownedWeapon[useWeaponIndex];
                        OnSelectedWeaponChanged?.Invoke(this, new OnSelectedWeaponChangedEventArgs
                        {
                            selectedWeapon = UseWeapon
                        });
                    }

                    AddMagazine(weaponToAdd.WeaponSO.WeaponType,weaponToAdd.NumberOfMagazine);
                    return true;
                }               
            }
            return false;
        }

        public int GetUseAmmo() => AmmoAmount[UseWeapon.WeaponType];

        public int GetUseMagazine() => MagazineAmount[UseWeapon.WeaponType];

        public void AddMagazine(WeaponType typeOfWeapon , int magazineNumber)
        {
            if (MagazineAmount[typeOfWeapon] + magazineNumber < maxAmmoMagazine[typeOfWeapon])
                MagazineAmount[typeOfWeapon] += magazineNumber;
            else
                MagazineAmount[typeOfWeapon] = maxAmmoMagazine[typeOfWeapon];

            OnAmmoChanged?.Invoke(this, EventArgs.Empty);
        }
        public void SubstractAmmo()
        {
            if (GetUseAmmo() > 0)
                AmmoAmount[UseWeapon.WeaponType]--;

            OnAmmoChanged?.Invoke(this, EventArgs.Empty);
        }


        private void DropUseWeapon()
        {
            if (!UseWeapon) return;

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
            if (!UseWeapon) return false;

            if (GetUseAmmo() <= 0)
            {
                isReload = true;
                return false;
            }

            return true;
        }

        private bool CanReload() => isReload && UseWeapon && GetUseMagazine() > 0;

        private void Reload()
        {
          
           MagazineAmount[UseWeapon.WeaponType]--;
           AmmoAmount[UseWeapon.WeaponType] = UseWeapon.AmmoInMagazine;
            
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
