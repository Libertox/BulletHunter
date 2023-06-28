using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shooter
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObject/Weapon", order = 1)]
    public class WeaponSO : ScriptableObject
    {
        public Mesh firstMesh;
        public Material[] firstPartMaterials;

        public Mesh secondMesh;
        public Material[] secondPartMaterials;

        public Weapon weapon;
      

    }
}
