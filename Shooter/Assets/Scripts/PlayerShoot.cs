using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shooter
{
    public class PlayerShoot:MonoBehaviour
    {
        public WeaponSO weaponSO;

        public MeshFilter firstMeshFilter;
        public MeshRenderer firstMeshRender;

        public MeshFilter secondMeshFiler;
        public MeshRenderer secondMeshRender;

        public Transform dropWeaponPosition;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {

                firstMeshFilter.mesh = weaponSO.firstMesh;
                firstMeshRender.materials = weaponSO.firstPartMaterials;

                secondMeshFiler.mesh = weaponSO.secondMesh;
                secondMeshRender.materials = weaponSO.secondPartMaterials;

            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                Weapon weapon = Instantiate(weaponSO.weapon, dropWeaponPosition.position, Quaternion.identity);
                weapon.Drop();
            }

        }

    }
}
