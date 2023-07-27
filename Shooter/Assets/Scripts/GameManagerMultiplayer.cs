using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Shooter
{
    public class GameManagerMultiplayer: NetworkBehaviour
    {
        public static GameManagerMultiplayer Instance { get; private set; }

        private NetworkList<ulong> playersIdList;

        private void Awake()
        {
            Instance = this;

            playersIdList = new NetworkList<ulong>();

            DontDestroyOnLoad(gameObject);

        }

        public override void OnNetworkSpawn()
        {
            NetworkManager.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallback;
        }

        private void NetworkManager_OnClientConnectedCallback(ulong clientId)
        {
            if (!playersIdList.Contains(clientId))
                playersIdList.Add(clientId);
        }

        public int GetIndexFromPlayerIdList(ulong playerId) => playersIdList.IndexOf(playerId);

    }
}
