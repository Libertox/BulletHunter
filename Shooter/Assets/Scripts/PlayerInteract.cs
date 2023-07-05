using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shooter
{
    public class PlayerInteract: MonoBehaviour
    {

        [SerializeField] private PlayerController playerController;
        [SerializeField] private float interactRange;

        [SerializeField] private LayerMask interactableLayerMask;
        [SerializeField] private LayerMask groundLayerMask;

        [SerializeField] private BoxCollider squatColider;
        [SerializeField] private BoxCollider uprightColider;
        

        private void Start()
        {
            GameInput.Instance.OnInteract += GameInput_OnInteract;
            PlayerController.OnSquated += PlayerController_OnSquated;
        }

        private void PlayerController_OnSquated(object sender, PlayerController.OnSquatedEventArgs e)
        {
            if (e.isSquat)
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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
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
            if (collider.TryGetComponent(out IInteractable interactableObject))
                interactableObject.Interact(playerController);
        }

        public bool GroundCheck()
        {
            Vector3 boxSize = new Vector3(0.6f, 0.6f, 0.6f);
            if (Physics.CheckBox(transform.position, boxSize, Quaternion.identity, groundLayerMask))
                return true;

            return false;
        }


    }
}
