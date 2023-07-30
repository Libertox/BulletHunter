using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Shooter
{
    public class MainMenuCleanUp:MonoBehaviour
    {
        private void Awake()
        {
            if (NetworkManager.Singleton != null)
                Destroy(NetworkManager.Singleton.gameObject);

            if (GameManagerMultiplayer.Instance != null)
                Destroy(GameManagerMultiplayer.Instance.gameObject);
        }

    }
}
