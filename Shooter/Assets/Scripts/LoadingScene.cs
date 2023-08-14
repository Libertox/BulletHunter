using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


namespace BulletHaunter.UI
{
    public class LoadingScene:NetworkBehaviour
    {
        [SerializeField] private float loadTime;
        [SerializeField] private BarUI loadingBar;

        private NetworkVariable<float> time = new NetworkVariable<float>();

        private void Update()
        {
            if (!NetworkManager.Singleton.IsServer) return;

            time.Value += Time.deltaTime;
            ChangeProgressBarClientRpc();
            if (time.Value > loadTime)
                SceneLoader.LoadNetwork(SceneLoader.GameScene.Game);    
        }

        [ClientRpc()]
        private void ChangeProgressBarClientRpc() => loadingBar.ChangeFillAmountImmediately(time.Value / loadTime);
    }
}
