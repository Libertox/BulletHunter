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

        private void Start()
        {
            GameInput.Instance.OnInteract += GameInput_OnInteract;

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
