using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Shooter
{
    public class WeaponVisual : MonoBehaviour
    {
        [SerializeField] private MeshFilter scopeMeshFilter;
        [SerializeField] private MeshRenderer scopeMeshRender;

        [SerializeField] private MeshFilter weaponMeshFiler;
        [SerializeField] private MeshRenderer weaponMeshRender;

        private void Start()
        {
            InventoryManager.Instance.OnSelectedWeaponDroped += Inventory_OnSelectedWeaponDroped;
        }

        private void Inventory_OnSelectedWeaponDroped(object sender, InventoryManager.OnSelectedWeaponChangedEventArgs e)
        {
            Gun weapon = Instantiate(e.selectedWeapon.WeaponSO.WeaponPrefab, transform.position, Quaternion.identity);
            weapon.SetAmmoAmount(e.selectedWeapon.AmmoAmount);
            weapon.Drop();
            SwapWeaponModel(null);
        }

        public void SwapWeaponModel(WeaponSO useWeapon)
        {
            if (useWeapon)
            {
                scopeMeshFilter.mesh = useWeapon.ScopeMesh;
                scopeMeshRender.materials = useWeapon.ScopePartMaterials;
                weaponMeshFiler.mesh = useWeapon.WeaponMesh;
                weaponMeshRender.materials = useWeapon.WeaponPartMaterials;
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
    }
}
