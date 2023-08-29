using System;
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
            if (IsOwner) 
            {
                GameInput.Instance.OnInteract += GameInput_OnInteract;
                PlayerController.OnSquated += PlayerController_OnSquated;
                GameManager.Instance.OnPlayerReconnected += GameManager_OnPlayerReconnected;
                if (PlayerStats.Instance != null)
                {
                    PlayerStats.Instance.OnDeathed += PlayerStats_OnDeathed;
                    PlayerStats.Instance.OnRestored += PlayerStats_OnRestored;
                }                
                else
                    PlayerStats.OnAnyPlayerSpawn += PlayerStats_OnAnyPlayerSpawn;
            }
            
        }

        private void GameManager_OnPlayerReconnected(object sender, EventArgs e)
        {
            SendColiderStatusServerRpc(uprightColider.enabled, squatColider.enabled, deathColider.enabled);
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

        private void PlayerStats_OnDeathed(object sender, EventArgs e) => SendColiderStatusServerRpc(false, false, true);
       
        private void PlayerStats_OnRestored(object sender, EventArgs e) => SendColiderStatusServerRpc(true, false, false);


        private void PlayerController_OnSquated(object sender, PlayerController.OnStateChangedEventArgs e) 
        {
            if (e.state)
                SendColiderStatusServerRpc(false, true, false);
            else
                SendColiderStatusServerRpc(true, false, false);
        }

        [ServerRpc(RequireOwnership = false)]
        private void SendColiderStatusServerRpc(bool uprightColiderStatus, bool squatColiderStatus, bool deathColiderStatus)
        {
            SendColiderStatusClientRpc(uprightColiderStatus, squatColiderStatus, deathColiderStatus);
        }

        [ClientRpc()]
        private void SendColiderStatusClientRpc(bool uprightColiderStatus, bool squatColiderStatus, bool deathColiderStatus)
        {
            uprightColider.enabled = uprightColiderStatus;
            squatColider.enabled = squatColiderStatus;
            deathColider.enabled = deathColiderStatus;
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
