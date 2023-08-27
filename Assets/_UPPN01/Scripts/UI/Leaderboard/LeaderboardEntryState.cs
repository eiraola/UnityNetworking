using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public struct LeaderboardEntryState: INetworkSerializable, IEquatable<LeaderboardEntryState> {

    public ulong ClientId;
    public FixedString32Bytes PlayerName;
    public int Coins;

    public bool Equals(LeaderboardEntryState other)
    {
        return ClientId == other.ClientId &&
             Coins == other.Coins &&
             PlayerName.Equals(other.PlayerName);
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref ClientId);
        serializer.SerializeValue(ref PlayerName);
        serializer.SerializeValue(ref Coins);
    }
}
