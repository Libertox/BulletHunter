using System;
using System.Collections.Generic;
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
       
        public enum GameScene
        {
            Game,
            MainMenu,
            LoadingScene,
        }


    }
}
