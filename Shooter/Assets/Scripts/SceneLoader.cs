using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BulletHaunter
{
    public static class SceneLoader
    {

        public static void Load(GameScene sceneTarget)
        {
            CleanUpStaticObject(sceneTarget);

            SceneManager.LoadScene(sceneTarget.ToString());
        }

        public static void LoadNetwork(GameScene sceneTarget)
        {
            CleanUpStaticObject(sceneTarget);
            NetworkManager.Singleton.SceneManager.LoadScene(sceneTarget.ToString(), LoadSceneMode.Single);
        }

        private static void CleanUpStaticObject(GameScene sceneTarget)
        {
            if (sceneTarget == GameScene.MainMenu)
            {
                if (NetworkManager.Singleton != null)
                    MonoBehaviour.Destroy(NetworkManager.Singleton.gameObject);

                if (GameManagerMultiplayer.Instance != null)
                    MonoBehaviour.Destroy(GameManagerMultiplayer.Instance.gameObject);

                if (LobbyManager.Instance != null)
                    MonoBehaviour.Destroy(LobbyManager.Instance.gameObject);

            }
        }
 
        public enum GameScene
        {
            Game,
            MainMenu,
            LoadingScene,
            WinningScene,
            TeamSelectScene,
            CharacterSelectScene,
        }


    }
}
