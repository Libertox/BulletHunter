using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Shooter
{
    public class WeaponVisual : NetworkBehaviour
    {
        [SerializeField] private MeshFilter scopeMeshFilter;
        [SerializeField] private MeshRenderer scopeMeshRender;

        [SerializeField] private MeshFilter weaponMeshFiler;
        [SerializeField] private MeshRenderer weaponMeshRender;

        private void Start()
        { 
            if(IsOwner)
                InventoryManager.Instance.OnSelectedWeaponDroped += Inventory_OnSelectedWeaponDroped;
        }

        private void Inventory_OnSelectedWeaponDroped(object sender, InventoryManager.OnSelectedWeaponChangedEventArgs e)
        {
            DropWeaponServerRpc(GameManager.Instance.GetWeaponSOIndex(e.selectedWeapon.WeaponSO), e.selectedWeapon.AmmoAmount);
            SwapWeaponModel(null);
        }

        [ServerRpc(RequireOwnership = false)]
        private void DropWeaponServerRpc(int weaponSOIndex , int ammoAmount)
        {
            WeaponSO weaponSO = GameManager.Instance.GetWeaponSOFromIndex(weaponSOIndex);
            Gun gun = Instantiate(weaponSO.WeaponPrefab,transform.position,Quaternion.identity);

            NetworkObject networkObject = gun.GetComponent<NetworkObject>();
            networkObject.Spawn(true);

            gun.SetAmmoAmount(ammoAmount);
            gun.Drop();
        }



        public void SwapWeaponModel(WeaponSO useWeapon)
        {
            SwapWeaponModelServerRpc(GameManager.Instance.GetWeaponSOIndex(useWeapon));
        }

        [ServerRpc(RequireOwnership = false)]
        private void SwapWeaponModelServerRpc(int weaponSOIndex)
        {
            SwapWeaponModelClientRpc(weaponSOIndex);
        }

        [ClientRpc()]
        private void SwapWeaponModelClientRpc(int weaponSOIndex)
        {
            WeaponSO useWeapon = GameManager.Instance.GetWeaponSOFromIndex(weaponSOIndex);
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
