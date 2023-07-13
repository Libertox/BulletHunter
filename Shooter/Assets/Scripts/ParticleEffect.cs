﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Shooter
{
    public class ParticleEffect:MonoBehaviour
    {
        [SerializeField] private float lifeTime;

        public void Init(Vector3 position, ObjectPool<ParticleEffect> objectPool)
        {
            transform.position = position;
            StartCoroutine(DisactiveAfterTime(objectPool));
        }

        private IEnumerator DisactiveAfterTime(ObjectPool<ParticleEffect> objectPool)
        {
            yield return new WaitForSeconds(lifeTime);
            objectPool.Release(this);
        }

    }
}
