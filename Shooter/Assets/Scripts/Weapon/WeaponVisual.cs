using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace BulletHaunter
{
    public class WeaponVisual : NetworkBehaviour
    {
        [SerializeField] private MeshFilter scopeMeshFilter;
        [SerializeField] private MeshRenderer scopeMeshRender;

        [SerializeField] private MeshFilter weaponMeshFiler;
        [SerializeField] private MeshRenderer weaponMeshRender;

        [SerializeField] private GameLayerMaskSO gameLayerMaskSO;

        private void Start()
        {
            if (IsOwner)
            {
                InventoryManager.Instance.OnSelectedWeaponDroped += Inventory_OnSelectedWeaponDroped;
                GameManager.Instance.OnPlayerReconnected += GameManager_OnPlayerReconnected;
            }
                
           GameManagerMultiplayer.Instance.OnPlayerDataNetworkListChanged += GameManagerMultiplayer_OnPlayerDataNetworkListChanged;
           
            SetWeaponLayerMask();
        }

        private void GameManager_OnPlayerReconnected(object sender, EventArgs e) => 
            SwapWeaponModel(InventoryManager.Instance.UseWeapon?.WeaponSO);
       

        private void GameManagerMultiplayer_OnPlayerDataNetworkListChanged(object sender, EventArgs e) => 
            SetWeaponLayerMask();
       

        public override void OnDestroy() => 
            GameManagerMultiplayer.Instance.OnPlayerDataNetworkListChanged -= GameManagerMultiplayer_OnPlayerDataNetworkListChanged;
       
        private void Inventory_OnSelectedWeaponDroped(object sender, InventoryManager.OnSelectedWeaponChangedEventArgs e) => SwapWeaponModel(null);
        

        public void SwapWeaponModel(WeaponSO useWeapon) => SwapWeaponModelServerRpc(GameManager.Instance.GetWeaponSOIndex(useWeapon));


        [ServerRpc(RequireOwnership = false)]
        private void SwapWeaponModelServerRpc(int weaponSOIndex) => SwapWeaponModelClientRpc(weaponSOIndex);


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

        private void SetWeaponLayerMask() => SetWeaponLayerMaskServerRpc();
     

        [ServerRpc(RequireOwnership = false)]
        private void SetWeaponLayerMaskServerRpc() => SetWeaponLayerMaskClientRpc();
      

        [ClientRpc]
        private void SetWeaponLayerMaskClientRpc()
        {
            int index = GameManagerMultiplayer.Instance.GetPlayerDataIndexFromClientId(OwnerClientId);
            if (index == -1) return;
            LayerMask gunLayerMask = gameLayerMaskSO.PlayerGunLayerMask[index];

            scopeMeshFilter.gameObject.layer = gunLayerMask;
            weaponMeshFiler.gameObject.layer = gunLayerMask;
        }
    }
}
