using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shooter.UI
{
    [CreateAssetMenu(fileName = "PlayerPositionImage", menuName = "ScriptableObject/PlayerPositionImage", order = 2)]
    public class PlayerPositionUIImageSO: ScriptableObject
    {
        [field: SerializeField] public Sprite UprightPositionImage { get; private set; }
        [field: SerializeField] public Sprite SquatPositionImage { get; private set; }
        [field: SerializeField] public Sprite SprintPositionImage { get; private set; }

    }
}
