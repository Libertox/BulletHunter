using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;

namespace BulletHaunter
{

    public  struct PlayerData: IEquatable<PlayerData>, INetworkSerializable
    {
        public ulong clientId;
        public int teamColorId;
        public int playerSkinId;
        public FixedString64Bytes playerId;
        public FixedString64Bytes playerName;

        public bool Equals(PlayerData other)
        {
            return clientId == other.clientId && teamColorId == other.teamColorId && playerId == other.playerId 
                && playerSkinId == other.playerSkinId && playerName == other.playerName;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref clientId);
            serializer.SerializeValue(ref teamColorId);
            serializer.SerializeValue(ref playerId);
            serializer.SerializeValue(ref playerSkinId);
            serializer.SerializeValue(ref playerName);
        }
    }
}
