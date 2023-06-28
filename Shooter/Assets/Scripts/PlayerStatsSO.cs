using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shooter
{
    [CreateAssetMenu(fileName = "PlayerStats", menuName = "ScriptableObject/PlayerStats", order = 0)]
    public class PlayerStatsSO: ScriptableObject
    {
        [SerializeField] private float maxHealth;
        [SerializeField] private float maxStamina;
        [SerializeField] private int maxGunAmmo;
        [SerializeField] private int maxRifleAmmo;
        [SerializeField] private int maxGrenadeAmount;

        public float MaxHealth => maxHealth;
        public float MaxStamina => maxStamina;
        public int MaxGunAmmo => maxGunAmmo;
        public int MaxRifleAmmo => maxRifleAmmo;
        public int MaxGrenadeAmount => maxGrenadeAmount;

    }
}
