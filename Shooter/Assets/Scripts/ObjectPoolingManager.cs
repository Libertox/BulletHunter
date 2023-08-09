using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Pool;

namespace BulletHaunter
{
    public class ObjectPoolingManager: NetworkBehaviour
    {
        public static ObjectPoolingManager Instance { get; private set; }

        public ObjectPool<Grenade> GrenadePool { get; private set; }
        public ObjectPool<ParticleEffect> GrenadeExplosionPool { get; private set; }
        public ObjectPool<ParticleEffect> BulletTrackPool { get; private set; }
        public ObjectPool<ParticleEffect> ShootEffectPool { get; private set; }

        [SerializeField] private Grenade grenadePrefab;
        [SerializeField] private ParticleEffect grenadeExplosionPrefab;
        [SerializeField] private ParticleEffect bulletTrackPrefab;
        [SerializeField] private ParticleEffect shootEffectPrefab;


        private void Awake()
        {
            if (!Instance)
                Instance = this; 
        }

    }
}
