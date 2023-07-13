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

            GrenadePool = new ObjectPool<Grenade>(
                () => Instantiate(grenadePrefab), 
                (grenade) => grenade.gameObject.SetActive(true),
                (grenade) => grenade.gameObject.SetActive(false)
                );

            GrenadeExplosionPool = new ObjectPool<ParticleEffect>(
                () => Instantiate(grenadeExplosionPrefab),
                (effect) => effect.gameObject.SetActive(true),
                (effect) => effect.gameObject.SetActive(false)
                );

            BulletTrackPool = new ObjectPool<ParticleEffect>(
                () => Instantiate(bulletTrackPrefab),
                (effect) => effect.gameObject.SetActive(true),
                (effect) => effect.gameObject.SetActive(false)
                );

            ShootEffectPool = new ObjectPool<ParticleEffect>(
                () => Instantiate(shootEffectPrefab),
                (effect) => effect.gameObject.SetActive(true),
                (effect) => effect.gameObject.SetActive(false)
                );

        } 
    }
}
