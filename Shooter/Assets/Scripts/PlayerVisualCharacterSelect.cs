using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shooter
{
    public class PlayerVisualCharacterSelect:MonoBehaviour
    {
        [SerializeField] private SkinnedMeshRenderer playerMeshRenderer;

        private float previousMousePosition;
        private bool isDrag;

        private void Start()
        {
            CharacterSelectManager.Instance.OnSkinChanged += CharacterSelectManager_OnSkinChanged;
            SetSkinnedMeshRendererMaterials(CharacterSelectManager.Instance.GetChooseMaterial()); 

        }

        private void CharacterSelectManager_OnSkinChanged(object sender, CharacterSelectManager.OnSkinChangedEventArgs e)
        {
            SetSkinnedMeshRendererMaterials(e.material);
        }

        private void SetSkinnedMeshRendererMaterials(Material material)
        {
            playerMeshRenderer.materials = new Material[]
            {
                material,
                material,
                material
            };

        }



        private void OnMouseDrag()
        {
            if (!isDrag)
            {
                previousMousePosition = Input.mousePosition.x;
                isDrag = true;
            }

            if (previousMousePosition > Input.mousePosition.x)
                transform.Rotate(new Vector3(0, -.5f, 0));
            else
                transform.Rotate(new Vector3(0, .5f, 0));
        }

        private void OnMouseUp()
        {
            isDrag = false;
        }


    }
}
