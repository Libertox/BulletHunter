using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Shooter.UI
{
    public class PlayerPositionUI: MonoBehaviour
    {
        [SerializeField] private Image playerPositionImage;
        [SerializeField] private PlayerPositionImageSO playerPositionImageSO;

        private void Start()
        {
            PlayerController.OnSquated += PlayerController_OnSquated;
            PlayerController.OnSprinted += PlayerController_OnSprinted;
        }

        private void PlayerController_OnSprinted(object sender, PlayerController.OnSprintedEventArgs e)
        {
            if(e.isSprint)
            {
                playerPositionImage.sprite = playerPositionImageSO.SprintPositionImage;
            }
            else
            {
                if (e.isSquat)
                    playerPositionImage.sprite = playerPositionImageSO.SquatPositionImage;
                else
                    playerPositionImage.sprite = playerPositionImageSO.UprightPositionImage;
            }

        }

        private void PlayerController_OnSquated(object sender, PlayerController.OnSquatedEventArgs e)
        {
            if (e.isSquat)
                playerPositionImage.sprite = playerPositionImageSO.SquatPositionImage;
            else
                playerPositionImage.sprite = playerPositionImageSO.UprightPositionImage;
        }
 
    }
}
