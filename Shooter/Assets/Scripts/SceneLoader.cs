using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Shooter
{
    public static class SceneLoader
    {

        public static void Load(GameScene sceneTarget)
        {
            SceneManager.LoadScene(sceneTarget.ToString());
        }

        public static void LoadNetwork(GameScene sceneTarget)
        {
            NetworkManager.Singleton.SceneManager.LoadScene(sceneTarget.ToString(), LoadSceneMode.Single);
        }
       
        public enum GameScene
        {
            Game,
            MainMenu,
            LoadingScene,
            LobbyScene,
            TeamSelectScene,
        }


    }
}
