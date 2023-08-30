using Unity.Netcode;
using UnityEngine;

namespace BulletHaunter
{
    public class PlayerDropWeapon: NetworkBehaviour
    {
        [SerializeField] private Transform dropWeaponPosition;

        private void Start()
        {
            if (IsOwner)
                InventoryManager.Instance.OnSelectedWeaponDroped += Inventory_OnSelectedWeaponDroped;      
        }

        private void Inventory_OnSelectedWeaponDroped(object sender, InventoryManager.OnSelectedWeaponChangedEventArgs e) => 
            DropWeaponServerRpc(GameManager.Instance.GetWeaponSOIndex(e.selectedWeapon.WeaponSO), e.selectedWeapon.AmmoAmount);
       
        [ServerRpc(RequireOwnership = false)]
        private void DropWeaponServerRpc(int weaponSOIndex, int ammoAmount)
        {
            WeaponSO weaponSO = GameManager.Instance.GetWeaponSOFromIndex(weaponSOIndex);
            Gun gun = Instantiate(weaponSO.WeaponPrefab, dropWeaponPosition.position, Quaternion.identity);

            NetworkObject networkObject = gun.GetComponent<NetworkObject>();
            networkObject.Spawn(true);

            gun.SetAmmoAmount(ammoAmount);
            gun.Drop();
        }

    }
}
