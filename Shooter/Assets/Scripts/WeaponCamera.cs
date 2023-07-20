using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace Shooter
{
    public class WeaponCamera: NetworkBehaviour
    {
        private void Start()
        {
            if (!IsOwner) gameObject.SetActive(false);
        }

    }
}
