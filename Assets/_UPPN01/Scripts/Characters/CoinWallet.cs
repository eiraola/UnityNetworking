using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class CoinWallet : NetworkBehaviour
{
    public NetworkVariable<int> totalCoins = new NetworkVariable<int>();

    private void OnTriggerEnter(Collider other)
    {
        Debug.LogError("xd");
        if (other.TryGetComponent<RespawningCoin>(out RespawningCoin col))
        {
            if (!IsServer)
            {
                col.Collect();
                return;
            }
            totalCoins.Value += col.Collect();
        }
    }
    public bool SpendCoins(int value)
    {
        if (totalCoins.Value < value)
        {
            return false;
        }
        if (IsServer)
        {
            totalCoins.Value -= value;
        }
        return true;
    }

}
