using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace BulletHaunter
{
    public class WeaponVisualPosition : NetworkBehaviour
    {
        [SerializeField] private Transform upPointTransform;
        [SerializeField] private Transform downPointTransform;
        [SerializeField] private Transform aimPointTransform;
        [SerializeField] private Transform normalPointTransform;


        [SerializeField] private float swapModelSpeed;
        [SerializeField] private float aimSpeed;

        [SerializeField] private WeaponVisual weaponVisual;

        private bool isAimed;
        private bool isChangePosition;

        private PositionState swapModleState;
        private enum PositionState
        {
            Down,
            Up,
        }

        private void Start()
        {
            InventoryManager.Instance.OnSelectedWeaponChanged += Inventory_OnSelectedWeaponChanged;

            GameInput.Instance.OnAimed += GameInput_OnAimed;
            GameInput.Instance.OnCancelAimed += GameInput_OnCancelAimed;
        }

        private void Inventory_OnSelectedWeaponChanged(object sender, InventoryManager.OnSelectedWeaponChangedEventArgs e)
        {
            isChangePosition = true;
            swapModleState = PositionState.Down;
        }

        private void GameInput_OnCancelAimed(object sender, EventArgs e) => isAimed = false;

        private void GameInput_OnAimed(object sender, EventArgs e) => isAimed = true;

        private void Update()
        {
            if (!IsOwner) return;

            HandleWeaponPositon();

            HandleAimPosition();
        }

        private void HandleWeaponPositon()
        {
            if (!isChangePosition) return;

            switch (swapModleState)
            {
                case PositionState.Down:
                    if (MoveDown())
                    {
                        weaponVisual.SwapWeaponModel(InventoryManager.Instance.UseWeapon?.WeaponSO);
                        swapModleState = PositionState.Up;
                    }
                    break;
                case PositionState.Up:
                    if (MoveUp())
                        isChangePosition = false;
                    break;

            }
        }
        private void HandleAimPosition()
        {
            if (isChangePosition) return;

            if (isAimed)
                transform.position = Vector3.Lerp(transform.position, aimPointTransform.position, Time.deltaTime * aimSpeed);
            else
                transform.position = Vector3.Lerp(transform.position, normalPointTransform.position, Time.deltaTime * aimSpeed);
        }


        public bool MoveDown()
        {
            transform.position = Vector3.Lerp(transform.position, downPointTransform.position, Time.deltaTime * swapModelSpeed);
            return transform.position == downPointTransform.position;

        }

        public bool MoveUp()
        {
            transform.position = Vector3.Lerp(transform.position, upPointTransform.position, Time.deltaTime * swapModelSpeed);
            return transform.position == upPointTransform.position;   
        }
    }
}