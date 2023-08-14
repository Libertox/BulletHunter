using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Pool;

namespace Unity.Netcode
{
    public class NetworkObjectPool : NetworkBehaviour
    {
        public static NetworkObjectPool Singleton { get; private set; }

        [SerializeField]
        List<PoolConfigObject> PooledPrefabsList;

        HashSet<GameObject> m_Prefabs = new HashSet<GameObject>();

        Dictionary<GameObject, ObjectPool<NetworkObject>> m_PooledObjects = new Dictionary<GameObject, ObjectPool<NetworkObject>>();

        public void Awake()
        {
            if (Singleton != null && Singleton != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Singleton = this;
            }

            foreach (var configObject in PooledPrefabsList)
            {
                RegisterPrefabInternal(configObject.Prefab, configObject.PrewarmCount);
            }

        }


        public override void OnNetworkDespawn()
        {
            // Unregisters all objects in PooledPrefabsList from the cache.
            foreach (var prefab in m_Prefabs)
            {
                // Unregister Netcode Spawn handlers
                NetworkManager.Singleton.PrefabHandler.RemoveHandler(prefab);
                m_PooledObjects[prefab].Clear();
            }
            m_PooledObjects.Clear();
            m_Prefabs.Clear();
        }

        public NetworkObject GetNetworkObject(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            var networkObject = m_PooledObjects[prefab].Get();

            var noTransform = networkObject.transform;
            noTransform.position = position;
            noTransform.rotation = rotation;

            return networkObject;
        }

        public void ReturnNetworkObject(NetworkObject networkObject, GameObject prefab)
        {
            m_PooledObjects[prefab].Release(networkObject);
        }

        void RegisterPrefabInternal(GameObject prefab, int prewarmCount)
        {
            NetworkObject CreateFunc()
            {
                return Instantiate(prefab).GetComponent<NetworkObject>();
            }

            void ActionOnGet(NetworkObject networkObject)
            {
                networkObject.gameObject.SetActive(true);
            }

            void ActionOnRelease(NetworkObject networkObject)
            {
                networkObject.gameObject.SetActive(false);
            }

            void ActionOnDestroy(NetworkObject networkObject)
            {
                Destroy(networkObject.gameObject);
            }

            m_Prefabs.Add(prefab);

            // Create the pool
            m_PooledObjects[prefab] = new ObjectPool<NetworkObject>(CreateFunc, ActionOnGet, ActionOnRelease, ActionOnDestroy, defaultCapacity: prewarmCount);

            // Populate the pool
            var prewarmNetworkObjects = new List<NetworkObject>();
            for (var i = 0; i < prewarmCount; i++)
            {
                prewarmNetworkObjects.Add(m_PooledObjects[prefab].Get());
            }
            foreach (var networkObject in prewarmNetworkObjects)
            {
                m_PooledObjects[prefab].Release(networkObject);
            }

            // Register Netcode Spawn handlers
            NetworkManager.Singleton.PrefabHandler.AddHandler(prefab, new PooledPrefabInstanceHandler(prefab, this));
        }
    }

    [Serializable]
    struct PoolConfigObject
    {
        public GameObject Prefab;
        public int PrewarmCount;
    }

    class PooledPrefabInstanceHandler : INetworkPrefabInstanceHandler
    {
        GameObject m_Prefab;
        NetworkObjectPool m_Pool;

        public PooledPrefabInstanceHandler(GameObject prefab, NetworkObjectPool pool)
        {
            m_Prefab = prefab;
            m_Pool = pool;
        }

        NetworkObject INetworkPrefabInstanceHandler.Instantiate(ulong ownerClientId, Vector3 position, Quaternion rotation)
        {
            return m_Pool.GetNetworkObject(m_Prefab, position, rotation);
        }

        void INetworkPrefabInstanceHandler.Destroy(NetworkObject networkObject)
        {
            m_Pool.ReturnNetworkObject(networkObject, m_Prefab);
        }
    }

}