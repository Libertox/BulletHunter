using System;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHaunter
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObject/Weapon", order = 1)]
    public class WeaponSO : ScriptableObject
    {
        [field: SerializeField] public Mesh ScopeMesh { get; private set; }
        [field: SerializeField] public Material[] ScopePartMaterials { get; private set; }
        [field: SerializeField] public Mesh WeaponMesh { get; private set; }
        [field: SerializeField] public Material[] WeaponPartMaterials { get; private set; }
        [field: SerializeField] public Sprite WeaponIcon { get; private set; }
        [field: SerializeField] public Gun WeaponPrefab { get; private set; }
        [field: SerializeField] public WeaponType WeaponType { get; private set; }
        [field: SerializeField] public int AmmoInMagazine { get; private set; }
        [field: SerializeField] public float WeaponRange { get; private set; }
        [field: SerializeField] public float WeaponZoom { get; private set; }
        [field: SerializeField] public float ReloadTime { get; private set; }
        [field: SerializeField] public float ShootSpeed { get; private set; }
        [field: SerializeField] public float Damage { get; private set; }


    }

    public enum WeaponType
    {
        Gun,
        Rifle,
        Sniper,
        Shoutgun,
        Grenade
    }
}
