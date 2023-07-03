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

        public float MaxHealth => maxHealth;
        public float MaxStamina => maxStamina;


    }
}
