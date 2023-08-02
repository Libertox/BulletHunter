using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Shooter
{
    public class PlayerThrowing:NetworkBehaviour
    {

        [SerializeField] private Transform throwTransform;
        [SerializeField] private Transform orientationPoint;
        [SerializeField] private TrajectoryLine trajectoryLine;

        [SerializeField] private Grenade grenadePrefab;

        [SerializeField] private Camera playerCamera;

        private bool isThrowed;

        private void Start()
        {
            if (IsOwner)
            {
                GameInput.Instance.OnThrowed += GameInput_OnThrowed;
                GameInput.Instance.OnCancelThrowed += GameInput_OnCancelThrowed;
            }     
        }

        private void Update()
        {
            if (!isThrowed || !IsOwner) return;

            trajectoryLine.DrawLine(throwTransform.transform.position, playerCamera.transform.forward * grenadePrefab.ThrowForce * grenadePrefab.Mass);

          
        }

        private void GameInput_OnCancelThrowed(object sender, EventArgs e)
        {
            if (InventoryManager.Instance.CanThrowGrenade())
            {
                isThrowed = false;
                trajectoryLine.Hide();
                ThowGrenadeServerRpc();
                InventoryManager.Instance.SubstractGranade();
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void ThowGrenadeServerRpc(ServerRpcParams serverRpcParams = default)
        {
            NetworkObject networkObject = NetworkObjectPool.Singleton.GetNetworkObject(grenadePrefab.gameObject, throwTransform.position, Quaternion.identity);
            Grenade grenade = networkObject.GetComponent<Grenade>();
            grenade.Throw(playerCamera.transform.forward);
            grenade.SetPrefab(grenadePrefab.gameObject);
            grenade.playerid = serverRpcParams.Receive.SenderClientId;
            networkObject.Spawn(true);
        }

        private void GameInput_OnThrowed(object sender, EventArgs e)
        {
            if(InventoryManager.Instance.CanThrowGrenade())
                   isThrowed = true;
        }
    }
}
