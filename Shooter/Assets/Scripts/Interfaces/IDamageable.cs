using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Shooter
{
    public interface IDamageable
    {
        public void TakeDamage();

        public NetworkObject GetNetworkObject();
    }
}
