using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace BulletHaunter
{
    public class ParticleEffect:NetworkBehaviour
    {
        [SerializeField] private float lifeTime;

        public void ReleaseToPool(GameObject prefab) => StartCoroutine(DisactiveCoroutine(prefab));

        private IEnumerator DisactiveCoroutine(GameObject prefab)
        {
            yield return new WaitForSeconds(lifeTime);
            NetworkObject.Despawn(false);
            NetworkObjectPool.Singleton.ReturnNetworkObject(NetworkObject, prefab);
        }

    }
}
