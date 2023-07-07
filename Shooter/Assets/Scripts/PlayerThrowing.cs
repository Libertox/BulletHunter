using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shooter
{
    public class PlayerThrowing:MonoBehaviour
    {

        [SerializeField] private Transform throwTransform;
        [SerializeField] private Transform orientationPoint;
        [SerializeField] private TrajectoryLine trajectoryLine;

        [SerializeField] private Grenade grenadePrefab;

        private bool isThrowed;

        private void Start()
        {
            GameInput.Instance.OnThrowed += GameInput_OnThrowed;
            GameInput.Instance.OnCancelThrowed += GameInput_OnCancelThrowed;
        }

        private void Update()
        {
            if (!isThrowed) return;

            trajectoryLine.DrawLine(throwTransform.transform.position, Camera.main.transform.forward * grenadePrefab.ThrowForce * grenadePrefab.Mass);

          
        }

        private void GameInput_OnCancelThrowed(object sender, EventArgs e)
        {
            if (Inventory.Instance.CanThrowGrenade() || true)
            {
                isThrowed = false;
                trajectoryLine.Hide();
                Grenade grenade = ObjectPoolingManager.Instance.GrenadePool.Get();
                grenade.Init(throwTransform.position);
                grenade.Throw(Camera.main.transform.forward);
                Inventory.Instance.SubstractGranade();
            }
        }

        private void GameInput_OnThrowed(object sender, EventArgs e)
        {
            if(Inventory.Instance.CanThrowGrenade() || true)
                   isThrowed = true;
        }
    }
}
