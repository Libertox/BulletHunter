using System;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHaunter
{
    public class CharacterSelectManager:MonoBehaviour
    {
        private const string PLAYER_PREFS_PLAYER_NAME = "PlayerName";
        private const string PLAYER_PREFS_CHOOSE_SKIN_INDEX = "ChooseSkinIndex";

        public static CharacterSelectManager Instance { get; private set; }

        public event EventHandler<OnSkinChangedEventArgs> OnSkinChanged;

        public class OnSkinChangedEventArgs : EventArgs { public Material material; }

        [SerializeField] private PlayerSkinsSO playerSkinsSO;

        public int ChooseSkinIndex { get; private set; }

        private void Awake()
        {
            Instance = this;
            ChooseSkinIndex = PlayerPrefs.GetInt(PLAYER_PREFS_CHOOSE_SKIN_INDEX);
        }

       
        public void SaveCharacterSelect(string playerName)
        {
           PlayerPrefs.SetString(PLAYER_PREFS_PLAYER_NAME, playerName);
           PlayerPrefs.SetInt(PLAYER_PREFS_CHOOSE_SKIN_INDEX, ChooseSkinIndex);
           PlayerPrefs.Save();
        }

        public string GetPlayerName() => PlayerPrefs.GetString(PLAYER_PREFS_PLAYER_NAME, "Player");
      

        public void SetNextSkin()
        {
            ChooseSkinIndex++;
            if (ChooseSkinIndex >= playerSkinsSO.PlayerSkinList.Count)
                ChooseSkinIndex = 0;

            OnSkinChanged?.Invoke(this, new OnSkinChangedEventArgs { material = GetChooseMaterial() });
        }

        public void SetPreviousSkin()
        {
            ChooseSkinIndex--;
            if (ChooseSkinIndex < 0)
                ChooseSkinIndex = playerSkinsSO.PlayerSkinList.Count - 1;

            OnSkinChanged?.Invoke(this, new OnSkinChangedEventArgs { material = GetChooseMaterial() });
        }

        public void SetChooseSkinIndex(int newIndex) 
        {
            ChooseSkinIndex = newIndex;

            OnSkinChanged?.Invoke(this, new OnSkinChangedEventArgs { material = GetChooseMaterial() });
        }

        public Material GetChooseMaterial() => playerSkinsSO.PlayerSkinList[ChooseSkinIndex];

    }
}
