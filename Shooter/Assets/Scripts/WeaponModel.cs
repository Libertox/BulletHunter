using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Shooter
{
    public class WeaponModel: MonoBehaviour
    {
        [SerializeField] private MeshFilter scopeMeshFilter;
        [SerializeField] private MeshRenderer scopeMeshRender;

        [SerializeField] private MeshFilter weaponMeshFiler;
        [SerializeField] private MeshRenderer weaponMeshRender;

        [SerializeField] private Transform upPointTransform;
        [SerializeField] private Transform downPointTransform;
        [SerializeField] private Transform aimPointTransform;
        [SerializeField] private Transform normalPointTransform;


        [SerializeField] private float swapModelSpeed;
        [SerializeField] private float aimSpeed;

        private bool isChangeModel;
        private bool isAimed;

        private SwapModleState swapModleState;

        private enum SwapModleState
        {
            MoveDown,
            MoveUp,
        }

        private void Start()
        {
            Inventory.Instance.OnSelectedWeaponChanged += Inventory_OnSelectedWeaponChanged;
            Inventory.Instance.OnSelectedWeaponDroped += Inventory_OnSelectedWeaponDroped;

            GameInput.Instance.OnAimed += GameInput_OnAimed;
            GameInput.Instance.OnCancelAimed += GameInput_OnCancelAimed;
        }

        private void GameInput_OnCancelAimed(object sender, EventArgs e) => isAimed = false;
   
        private void GameInput_OnAimed(object sender, EventArgs e) => isAimed = true;
       
        private void Inventory_OnSelectedWeaponDroped(object sender, Inventory.OnSelectedWeaponChangedEventArgs e)
        {
            Weapon weapon = Instantiate(e.selectedWeapon.WeaponSO.WeaponPrefab, transform.position, Quaternion.identity);
            weapon.SetAmmoAmount(e.selectedWeapon.AmmoAmount);
            weapon.Drop();
            SwapWeaponModel(null);
        }

        private void Inventory_OnSelectedWeaponChanged(object sender, Inventory.OnSelectedWeaponChangedEventArgs e)
        {
            isChangeModel = true;
            swapModleState = SwapModleState.MoveDown; 
        }

        private void Update()
        {
            SwapWeaponModelStateMachine();

            HandleAimPosition();
        }

        private void HandleAimPosition()
        {
            if (isChangeModel) return;

            if (isAimed)
                transform.position = Vector3.Lerp(transform.position, aimPointTransform.position, Time.deltaTime * aimSpeed);
            else
                transform.position = Vector3.Lerp(transform.position, normalPointTransform.position, Time.deltaTime * aimSpeed);
        }

        private void SwapWeaponModelStateMachine()
        {
            if (!isChangeModel) return;

            switch (swapModleState)
            {
                case SwapModleState.MoveDown:
                    transform.position = Vector3.Lerp(transform.position, downPointTransform.position, Time.deltaTime * swapModelSpeed);
                    if (transform.position == downPointTransform.position)
                    {
                        SwapWeaponModel(Inventory.Instance.UseWeapon?.WeaponSO);
                        swapModleState = SwapModleState.MoveUp;
                    }
                    break;
                case SwapModleState.MoveUp:
                    transform.position = Vector3.Lerp(transform.position, upPointTransform.position, Time.deltaTime * swapModelSpeed);
                    if (transform.position == upPointTransform.position)
                    {
                        isChangeModel = false;
                    }
                    break;

            }
        }

        private void SwapWeaponModel(WeaponSO useWeapon)
        {
            if (useWeapon)
            {
                scopeMeshFilter.mesh = useWeapon.ScopeMesh;
                scopeMeshRender.materials = useWeapon.ScopePartMaterials;
                weaponMeshFiler.mesh = useWeapon.WeaponMesh;
                weaponMeshRender.materials = useWeapon.WeaponPartMaterials;
                ShowWeaponModel();
            }
            else
                HideWeaponModel();

        }

        private void HideWeaponModel()
        {
            scopeMeshFilter.gameObject.SetActive(false);
            weaponMeshFiler.gameObject.SetActive(false);
        }

        private void ShowWeaponModel()
        {
            scopeMeshFilter.gameObject.SetActive(true);
            weaponMeshFiler.gameObject.SetActive(true);
        }

    }
}
