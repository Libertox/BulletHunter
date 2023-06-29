using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shooter
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObject/Weapon", order = 1)]
    public class WeaponSO : ScriptableObject
    {
        [SerializeField] private Mesh scopeMesh;
        [SerializeField] private Material[] scopePartMaterials;
        [SerializeField] private Mesh weaponMesh;
        [SerializeField] private Material[] weaponPartMaterials;
        [SerializeField] private Weapon weaponPrefab;

        public Mesh ScopeMesh => scopeMesh;
        public Material[] ScopePartMaterials => scopePartMaterials;

        public Mesh WeaponMesh => weaponMesh;
        public Material[] WeaponPartMaterials => weaponPartMaterials;

        public Weapon WeaponPrefab => weaponPrefab;




    }
}
