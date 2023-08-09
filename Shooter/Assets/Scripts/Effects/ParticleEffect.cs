using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

namespace BulletHaunter
{
    public class ParticleEffect:NetworkBehaviour
    {
        [SerializeField] private float lifeTime;

        public void Release(GameObject prefab) => StartCoroutine(DisactiveAfterTime(prefab));

        private IEnumerator DisactiveAfterTime(GameObject prefab)
        {
            yield return new WaitForSeconds(lifeTime);
            NetworkObject.Despawn(false);
            NetworkObjectPool.Singleton.ReturnNetworkObject(NetworkObject, prefab);
        }

    }
}
