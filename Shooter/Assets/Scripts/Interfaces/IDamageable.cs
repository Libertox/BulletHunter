using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Shooter
{
    public interface IDamageable
    {
        public bool TakeDamage(float damage, ulong clientId);

        public NetworkObject GetNetworkObject();
    }
}
