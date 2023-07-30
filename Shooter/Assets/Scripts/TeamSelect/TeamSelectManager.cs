using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Shooter
{
    public class TeamSelectManager: NetworkBehaviour
    {
        public static TeamSelectManager Instance { get; private set; }

        public event EventHandler OnReadyChanged;


        private Dictionary<ulong, bool> playerReadyDictionary;

        private void Awake()
        {
            Instance = this;

            playerReadyDictionary = new Dictionary<ulong, bool>();
        }

        public void SetPlayerReady() => SetPlayerReadyServerRpc();

        [ServerRpc(RequireOwnership = false)]
        private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
        {
            SetPlayerReadyClientRpc(serverRpcParams.Receive.SenderClientId);
            playerReadyDictionary[serverRpcParams.Receive.SenderClientId] = true;

            bool allClientsReady = true;
            foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
            {
                if (!playerReadyDictionary.ContainsKey(clientId) || !playerReadyDictionary[clientId])
                {
                    allClientsReady = false;
                    break;
                }
            }

            if (allClientsReady)
            {
                SceneLoader.LoadNetwork(SceneLoader.GameScene.Game);
            }

            OnReadyChanged?.Invoke(this, EventArgs.Empty);

        }

        [ClientRpc()]
        private void SetPlayerReadyClientRpc(ulong clientId)
        {
            playerReadyDictionary[clientId] = true;

            OnReadyChanged?.Invoke(this, EventArgs.Empty);
        }


        public bool IsPlayerReady(ulong playerId) => playerReadyDictionary.ContainsKey(playerId) && playerReadyDictionary[playerId];


    }
}
