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
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit,interactRange,interactableLayerMask))
            {
                if(hit.transform.TryGetComponent(out IInteractable interactable))
                {
                    interactable.Interact(playerController);
                }
            }
        }
    }
}
