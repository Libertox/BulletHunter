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

        private readonly NetworkVariable<float> time = new NetworkVariable<float>();

        private void Start() => time.OnValueChanged += OnValueChanged;
      
        private void OnValueChanged(float previousValue, float newValue) => loadingBar.ChangeFillAmountImmediately(newValue / loadTime);
      
        private void Update()
        {
            if (!NetworkManager.Singleton.IsServer) return;

            time.Value += Time.deltaTime;
            if (time.Value > loadTime)
                SceneLoader.LoadNetwork(SceneLoader.GameScene.Game);                 
        }

    }
}
