using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Shooter
{
    public class ObjectPoolingManager: MonoBehaviour
    {
        public static ObjectPoolingManager Instance { get; private set; }

        public ObjectPool<Grenade> GrenadePool { get; private set; }
        public ObjectPool<ParticleEffect> GrenadeExplosionPool { get; private set; }

        [SerializeField] private Grenade grenadePrefab;
        [SerializeField] private ParticleEffect grenadeExplosionPrefab;

        private void Awake()
        {
            if (!Instance)
                Instance = this;

            GrenadePool = new ObjectPool<Grenade>(() => Instantiate(grenadePrefab));
            GrenadeExplosionPool = new ObjectPool<ParticleEffect>(() => Instantiate(grenadeExplosionPrefab));
        }


     
    }
}
