using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace Shooter
{
    public interface INetworkPrefabInstanceHandler
    {
        NetworkObject Instantiate(ulong ownerClientId, Vector3 position, Quaternion rotation);
        void Destroy(NetworkObject networkObject);
    }
}
