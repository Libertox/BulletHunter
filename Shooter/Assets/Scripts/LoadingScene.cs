using System;
using System.Collections.Generic;
using UnityEngine;


namespace Shooter.UI
{
    public class LoadingScene:MonoBehaviour
    {
        [SerializeField] private float loadTime;
        [SerializeField] private BarUI loadingBar;

        private float time;

        private void Update()
        {
            time += Time.deltaTime;
            loadingBar.ChangeFillAmount(time / loadTime);
            if (time > loadTime)
            {
                SceneLoader.Load(SceneLoader.GameScene.Game);
            }
                
        }

    }
}
