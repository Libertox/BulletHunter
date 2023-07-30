using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shooter
{
    public class TeamSelectPlatform:MonoBehaviour
    {
        [SerializeField] private MeshRenderer platformMeshRender;

        private Material material;

        private void Awake()
        {
            material = new Material(platformMeshRender.material);
            platformMeshRender.material = material;
        }


        public void SetColor(Color color) => material.color = color;

    }
}
