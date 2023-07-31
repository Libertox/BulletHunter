using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;

namespace Shooter
{

    public  struct PlayerData: IEquatable<PlayerData>, INetworkSerializable
    {
        public ulong clientId;
        public int teamColorId;
        public FixedString64Bytes playerId;

        public bool Equals(PlayerData other)
        {
            return clientId == other.clientId && teamColorId == other.teamColorId && playerId == other.playerId;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref clientId);
            serializer.SerializeValue(ref teamColorId);
            serializer.SerializeValue(ref playerId);
        }
    }
}
