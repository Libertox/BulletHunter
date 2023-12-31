﻿using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace BulletHaunter
{
    public interface IDamageable
    {
        public void TakeDamage(float damage, ulong clientId);

        public NetworkObject GetNetworkObject();
    }
}
