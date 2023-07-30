using System;
using System.Collections.Generic;
using Unity.Netcode;

namespace Shooter
{
    public  struct PlayerData: IEquatable<PlayerData>, INetworkSerializable
    {
        public ulong clientId;
        public int teamColorId;

        public bool Equals(PlayerData other)
        {
            return clientId == other.clientId && teamColorId == other.teamColorId;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref clientId);
            serializer.SerializeValue(ref teamColorId);
        }
    }
}
