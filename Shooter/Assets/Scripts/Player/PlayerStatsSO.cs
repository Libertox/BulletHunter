using System;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHaunter
{
    [CreateAssetMenu(fileName = "PlayerStats", menuName = "ScriptableObject/PlayerStats", order = 0)]
    public class PlayerStatsSO: ScriptableObject
    {
        [field: SerializeField] public float MaxHealth { get; private set; }

        [field: SerializeField] public float MaxStamina { get; private set; }

        [field: SerializeField] public float MaxArmor { get; private set; }

        [field: SerializeField] public float RestoreCooldown { get; private set; }

        [field: SerializeField] public float InvulnerabilityCooldown { get; private set; }
    }
}
