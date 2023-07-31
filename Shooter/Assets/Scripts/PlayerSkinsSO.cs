using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shooter
{
    [CreateAssetMenu(fileName = "PlayerSkins", menuName = "ScriptableObject/PlayerSkins", order = 2)]
    public class PlayerSkinsSO:ScriptableObject
    {
        [field: SerializeField] public List<Material> PlayerSkinList { get; private set; }

    }
}
