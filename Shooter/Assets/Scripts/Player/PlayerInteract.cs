using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Shooter
{
    public class PlayerInteract: NetworkBehaviour
    {

        [SerializeField] private PlayerController playerController;
        [SerializeField] private float interactRange;

        [SerializeField] private LayerMask interactableLayerMask;
        [SerializeField] private LayerMask groundLayerMask;

        [SerializeField] private BoxCollider squatColider;
        [SerializeField] private BoxCollider uprightColider;

        [SerializeField] private Camera playerCamera;

        private void Start()
        {
            GameInput.Instance.OnInteract += GameInput_OnInteract;

            if(IsOwner)
                PlayerController.OnSquated += PlayerController_OnSquated;
        }

        private void PlayerController_OnSquated(object sender, PlayerController.OnStateChangedEventArgs e)
        {
            ChangeColiderServerRpc(e.state);
        }

        [ServerRpc()]
        private void ChangeColiderServerRpc(bool state)
        {
            ChangeColiderClientRpc(state);
        }

        [ClientRpc()]
        private void ChangeColiderClientRpc(bool state)
        {
            if (state)
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
                {
                    interactable.Interact(playerController);
                }
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
                playerController.DropLadder();      
        }

        public bool GroundCheck()
        {
            Vector3 boxSize = new Vector3(1f, 0.1f, 1f);
            if (Physics.CheckBox(transform.position, boxSize, Quaternion.identity, groundLayerMask))
                return true;

            return false;
        }


    }
}
