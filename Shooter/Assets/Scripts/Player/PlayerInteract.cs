using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace BulletHaunter
{
    public class PlayerInteract: NetworkBehaviour
    {

        [SerializeField] private PlayerController playerController;
        [SerializeField] private float interactRange;

        [SerializeField] private LayerMask interactableLayerMask;
        [SerializeField] private LayerMask groundLayerMask;

        [SerializeField] private BoxCollider squatColider;
        [SerializeField] private BoxCollider uprightColider;
        [SerializeField] private BoxCollider deathColider;

        [SerializeField] private Camera playerCamera;

        private void Start()
        {
            GameInput.Instance.OnInteract += GameInput_OnInteract;

            if (IsOwner) 
            {
                PlayerController.OnSquated += PlayerController_OnSquated;

                if (PlayerStats.Instance != null)
                {
                    PlayerStats.Instance.OnDeathed += PlayerStats_OnDeathed;
                    PlayerStats.Instance.OnRestored += PlayerStats_OnRestored;
                }                
                else
                    PlayerStats.OnAnyPlayerSpawn += PlayerStats_OnAnyPlayerSpawn;
            }
               
        }

        private void PlayerStats_OnAnyPlayerSpawn(object sender, EventArgs e)
        {
            if (PlayerStats.Instance != null)
            {
                PlayerStats.Instance.OnDeathed -= PlayerStats_OnDeathed;
                PlayerStats.Instance.OnDeathed += PlayerStats_OnDeathed;

                PlayerStats.Instance.OnRestored -= PlayerStats_OnRestored;
                PlayerStats.Instance.OnRestored += PlayerStats_OnRestored;
            }
                
        }

        private void PlayerStats_OnDeathed(object sender, EventArgs e) => ChangeDeathColiderServerRpc();
       
        private void PlayerStats_OnRestored(object sender, EventArgs e) => SetUprightColiderServerRpc();
        

        [ServerRpc(RequireOwnership = false)]
        private void ChangeDeathColiderServerRpc() => ChangeDeathColiderClientRpc();
       

        [ClientRpc()]
        private void ChangeDeathColiderClientRpc()
        {
            uprightColider.enabled = false;
            squatColider.enabled = false;
            deathColider.enabled = true;
        }

        [ServerRpc(RequireOwnership = false)]
        private void SetUprightColiderServerRpc() => SetUprightColiderClientRpc();
        
        [ClientRpc]
        private void SetUprightColiderClientRpc()
        {
            uprightColider.enabled = true;
            squatColider.enabled = false;
            deathColider.enabled = false;
        }

        private void PlayerController_OnSquated(object sender, PlayerController.OnStateChangedEventArgs e) => ChangeSquatColiderServerRpc(e.state);
        

        [ServerRpc()]
        private void ChangeSquatColiderServerRpc(bool isSquat) => ChangeSquatColiderClientRpc(isSquat);
       

        [ClientRpc()]
        private void ChangeSquatColiderClientRpc(bool isSquat)
        {
            if (isSquat)
            {
                uprightColider.enabled = false;
                squatColider.enabled = true;
            }
            else
            {
                uprightColider.enabled = true;
                squatColider.enabled = false;
            }
        }

        private void GameInput_OnInteract(object sender, EventArgs e)
        {
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactableLayerMask))
            {
                if (hit.transform.TryGetComponent(out IInteractable interactable))
                    interactable.Interact(playerController);
            }
        }

        private void OnTriggerStay(Collider collider)
        {
            if (!IsOwner) return;

            if (collider.TryGetComponent(out IInteractable interactableObject))
                interactableObject.Interact(playerController);

        }

        
        private void OnTriggerExit(Collider collider)
        {
            if (!IsOwner) return;

            if (collider.GetComponent<Ladder>())
                playerController.GetOffLader();      
        }

        public bool GroundCheck()
        {
            Vector3 boxSize = new Vector3(1f, 0.1f, 1f);
            return (Physics.CheckBox(transform.position, boxSize, Quaternion.identity, groundLayerMask));
        }
    }
}
