using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public class CoinWallet : NetworkBehaviour
{
    [SerializeField]
    private BountyCoin bountyCoinPrefab;
    [SerializeField]
    private Health healthComponent;
    public NetworkVariable<int> totalCoins = new NetworkVariable<int>();
    [Header("Settings")]
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float bountyPercentage = 1.0f;
    [SerializeField]
    private int bountyCoinCount = 10;
    [SerializeField]
    private int minBountyCoinValue = 5;

    private void OnTriggerEnter(Collider other)
    {
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
    public override void OnNetworkSpawn()
    {
        if (!IsServer)
        {
            return;
        }
        healthComponent.OnDied += HandleDie;
    }
    public override void OnNetworkDespawn()
    {
        healthComponent.OnDied -= HandleDie;
    }
    private void HandleDie(Health health)
    {
        int bountyValue = (int)(totalCoins.Value * (bountyPercentage));
        int bountyCoinValue = bountyValue / bountyCoinCount;
        if (bountyCoinValue < minBountyCoinValue) { return; }
        for (int i = 0; i < bountyCoinCount; i++)
        {
            BountyCoin coinInstance = Instantiate(bountyCoinPrefab, transform.position, Quaternion.identity);
            coinInstance.SetValue(bountyCoinValue);
            coinInstance.GetComponent<NetworkObject>().Spawn();
        }
    }
}
