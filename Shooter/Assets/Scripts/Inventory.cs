using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shooter
{
    public class Inventory: MonoBehaviour
    {
        
        public static int MaxNumberWeapon { get; private set; } = 4;
        public WeaponSO UseWeapon { get; private set; }

        [SerializeField] private MeshFilter scopeMeshFilter;
        [SerializeField] private MeshRenderer scopeMeshRender;

        [SerializeField] private MeshFilter weaponMeshFiler;
        [SerializeField] private MeshRenderer weaponMeshRender;

        private WeaponSO[] ownedWeapon;
        private int useWeaponIndex;

        private void Awake()
        {
            ownedWeapon = new WeaponSO[MaxNumberWeapon];

        }
        private void Start()
        {
            GameInput.Instance.OnWeaponSelected += GameInput_OnWeaponSelected;
            GameInput.Instance.OnWeaponDroped += GameInput_OnWeaponDroped;
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
                        
                    //Dodawanie amunicji
                    return true;
                }               
            }
            return false;
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
