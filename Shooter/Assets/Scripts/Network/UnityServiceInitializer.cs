using System;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace BulletHaunter
{
    public class UnityServiceInitializer:MonoBehaviour
    {

        private void Awake() => InitializeUnityAuthentication();
      
        private async void InitializeUnityAuthentication()
        {
            if (UnityServices.State != ServicesInitializationState.Initialized)
            {
                await UnityServices.InitializeAsync();

                await AuthenticationService.Instance.SignInAnonymouslyAsync();

                SceneLoader.Load(SceneLoader.GameScene.MainMenu);
            }

        }
    }
}
