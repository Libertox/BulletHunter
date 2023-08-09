using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace BulletHaunter
{
    public interface INetworkPrefabInstanceHandler
    {
        NetworkObject Instantiate(ulong ownerClientId, Vector3 position, Quaternion rotation);
        void Destroy(NetworkObject networkObject);
    }
}
