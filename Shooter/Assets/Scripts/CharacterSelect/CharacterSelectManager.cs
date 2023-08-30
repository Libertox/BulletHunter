using System;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHaunter.CharacterSelect
{
    public class CharacterSelectManager:MonoBehaviour
    {
        public static CharacterSelectManager Instance { get; private set; }

        public event EventHandler<OnSkinChangedEventArgs> OnSkinChanged;

        public class OnSkinChangedEventArgs : EventArgs { public Material material; }
  
        [SerializeField] private PlayerSkinsSO playerSkinsSO;

        public int ChooseSkinIndex { get; private set; }

        private void Awake()
        {
            Instance = this;
            ChooseSkinIndex = PlayerPrefs.GetInt(GameManagerMultiplayer.PLAYER_PREFS_CHOOSE_SKIN_INDEX);
        }

        public void SaveCharacterSelect(string playerName)
        {
           PlayerPrefs.SetString(GameManagerMultiplayer.PLAYER_PREFS_PLAYER_NAME, playerName);
           PlayerPrefs.SetInt(GameManagerMultiplayer.PLAYER_PREFS_CHOOSE_SKIN_INDEX, ChooseSkinIndex);
           PlayerPrefs.Save();
        }

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

        public void SetSpecificSkin(int skinIndex) 
        {
            ChooseSkinIndex = skinIndex;

            OnSkinChanged?.Invoke(this, new OnSkinChangedEventArgs { material = GetChooseMaterial() });
        }

        public Material GetChooseMaterial() => playerSkinsSO.PlayerSkinList[ChooseSkinIndex];

    }
}
