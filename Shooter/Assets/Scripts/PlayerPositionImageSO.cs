using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shooter.UI
{
    [CreateAssetMenu(fileName = "PlayerPositionImage", menuName = "ScriptableObject/PlayerPositionImage", order = 2)]
    public class PlayerPositionImageSO: ScriptableObject
    {
        [SerializeField] private Sprite uprightPositionImage;
        [SerializeField] private Sprite squatPositionImage;
        [SerializeField] private Sprite sprintPositionImage;

        public Sprite UprightPositionImage => uprightPositionImage;
        public Sprite SquatPositionImage => squatPositionImage;
        public Sprite SprintPositionImage => sprintPositionImage;
    }
}
