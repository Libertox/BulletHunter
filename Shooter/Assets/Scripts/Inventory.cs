using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shooter
{
    public class Inventory: MonoBehaviour
    {
        private const int maxGunAmmo = 64;
        private const int maxShoutgunAmmo = 48;
        private const int maxRifleAmmo = 192;
        private const int maxSniperAmmo = 32;
        private const int maxGranadeAmmo = 5;
        
        public static int MaxNumberWeapon { get; private set; } = 4;
        public WeaponSO UseWeapon { get; private set; }

        [SerializeField] private MeshFilter scopeMeshFilter;
        [SerializeField] private MeshRenderer scopeMeshRender;

        [SerializeField] private MeshFilter weaponMeshFiler;
        [SerializeField] private MeshRenderer weaponMeshRender;

        private WeaponSO[] ownedWeapon;
        private int useWeaponIndex;

        public Dictionary<WeaponType, int> AmmoAmount { get; private set; }
        private Dictionary<WeaponType, int> maxAmmoAmount;


        private void Awake()
        {
            ownedWeapon = new WeaponSO[MaxNumberWeapon];
            AmmoAmount = new Dictionary<WeaponType, int>
            {
                {WeaponType.Gun,0 },
                {WeaponType.Rifle,0 },
                {WeaponType.Sniper,0 },
                {WeaponType.Shoutgun,0 },
                {WeaponType.Grenade,0 }
            };

            maxAmmoAmount = new Dictionary<WeaponType, int>
            {
                {WeaponType.Gun,maxGunAmmo },
                {WeaponType.Rifle,maxRifleAmmo},
                {WeaponType.Shoutgun,maxShoutgunAmmo},
                {WeaponType.Sniper,maxSniperAmmo },
                {WeaponType.Grenade,maxGranadeAmmo }
            };

        }
        private void Start()
        {
            GameInput.Instance.OnWeaponSelected += GameInput_OnWeaponSelected;
            GameInput.Instance.OnWeaponDroped += GameInput_OnWeaponDroped;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                foreach(var element in AmmoAmount)
                {
                    Debug.Log(element);
                }
            }
        }

        private void GameInput_OnWeaponDroped(object sender, EventArgs e) => DropUseWeapon();
     
        private void GameInput_OnWeaponSelected(object sender, GameInput.OnWeaponSelectedEventArgs e)
        {
            UseWeapon = ownedWeapon[e.selectWeaponIndex];
            useWeaponIndex = e.selectWeaponIndex;
            SwapWeaponModel();
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
                        SwapWeaponModel();
                    }

                    AddAmmo(weaponToAdd.WeaponSO.WeaponType,weaponToAdd.NumberOfAmmo);
                    return true;
                }               
            }
            return false;
        }

        public void AddAmmo(WeaponType typeOfAmmo , int ammoNumber)
        {
            int newAmmo = AmmoAmount[typeOfAmmo] + ammoNumber;
            if (newAmmo < maxAmmoAmount[typeOfAmmo])
                AmmoAmount[typeOfAmmo] += ammoNumber;
            else
                AmmoAmount[typeOfAmmo] = maxAmmoAmount[typeOfAmmo];
        }

        private void SwapWeaponModel()
        {
            if (UseWeapon)
            {
                scopeMeshFilter.mesh = UseWeapon.ScopeMesh;
                scopeMeshRender.materials = UseWeapon.ScopePartMaterials;
                weaponMeshFiler.mesh = UseWeapon.WeaponMesh;
                weaponMeshRender.materials = UseWeapon.WeaponPartMaterials;
                ShowWeaponModel();
            }
            else
                HideWeaponModel();

        }

        private void HideWeaponModel()
        {
            scopeMeshFilter.gameObject.SetActive(false);
            weaponMeshFiler.gameObject.SetActive(false);
        }

        private void ShowWeaponModel()
        {
            scopeMeshFilter.gameObject.SetActive(true);
            weaponMeshFiler.gameObject.SetActive(true);
        }

        private void DropUseWeapon()
        {
            if (!UseWeapon) return;

            HideWeaponModel();
            Weapon weapon = Instantiate(UseWeapon.WeaponPrefab, transform.position, Quaternion.identity);
            weapon.Drop();
            UseWeapon = null;
            ownedWeapon[useWeaponIndex] = null;
        }

    }
}
