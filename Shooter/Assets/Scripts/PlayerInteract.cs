using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shooter
{
    public class PlayerInteract: MonoBehaviour
    {

        [SerializeField] private PlayerController playerController;

        private void Start()
        {
            GameInput.Instance.OnInteract += GameInput_OnInteract;

        }

        private void GameInput_OnInteract(object sender, EventArgs e)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit,2f))
            {
                if(hit.transform.TryGetComponent(out Interactable interactable))
                {
                    interactable.Interact(playerController);
                }
            }
        }
    }
}
