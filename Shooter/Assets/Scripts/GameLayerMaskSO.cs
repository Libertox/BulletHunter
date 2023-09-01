using System;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHaunter
{
    [CreateAssetMenu(fileName = "GameLayerMask", menuName = "ScriptableObject/GameLayerMask", order = 3)]
    public class GameLayerMaskSO:ScriptableObject
    {
        [field: SerializeField] public int[] PlayerLayerMask { get; private set; }
        [field: SerializeField] public int[] PlayerGunLayerMask { get; private set; }
        [field: SerializeField] public int[] PlayerTeamLayerMask { get; private set; }

    }
}
