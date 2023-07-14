using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shooter
{
    public class PickupObjectSpawner:MonoBehaviour
    {
        [SerializeField] private float spawnDuration;
        [SerializeField] private PickupObject[] spawnObjects;

        private WaitForSeconds waitForSeconds;

        private void Start() 
        {
            waitForSeconds = new WaitForSeconds(spawnDuration);
            GetNewRandomPickupObject().gameObject.SetActive(true);
        } 
     

        private PickupObject GetNewRandomPickupObject()
        {
            int pickupObjectIndex = UnityEngine.Random.Range(0, spawnObjects.Length);
            return spawnObjects[pickupObjectIndex];
        }

        public void SpawnNewObject() => StartCoroutine(WaitForSpawnNewObject());

        private IEnumerator WaitForSpawnNewObject()
        {
            yield return waitForSeconds;
            GetNewRandomPickupObject().gameObject.SetActive(true);
        }
    }
}
