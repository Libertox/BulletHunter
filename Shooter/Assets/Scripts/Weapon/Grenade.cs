using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shooter
{
    public class Grenade: MonoBehaviour
    {
        [SerializeField] private Rigidbody rgb;
        [SerializeField] private Transform explosionParticleEffect;
        [SerializeField] private float rotationForce;
        [SerializeField] private float explosionTime;

        [field: SerializeField] public float ThrowForce { get; private set; }

        public float Mass => rgb.mass;


        public void Throw(Vector3 direction)
        {
            rgb.AddForce(direction * ThrowForce, ForceMode.Impulse);
            StartCoroutine(Explosion());
        }

        public void Init(Vector3 startPosition)
        {

            transform.SetPositionAndRotation(startPosition, new Quaternion(0, 0, 0, 0));
            rgb.velocity = Vector3.zero;
            rgb.angularVelocity = Vector3.zero;
            rgb.rotation = Quaternion.Euler(Vector3.zero);
        }

        private IEnumerator Explosion()
        {
            yield return new WaitForSeconds(explosionTime);
            ObjectPoolingManager.Instance.GrenadeExplosionPool.Get().Init(transform.position, ObjectPoolingManager.Instance.GrenadeExplosionPool);
            ObjectPoolingManager.Instance.GrenadePool.Release(this);

        }

    }
}
