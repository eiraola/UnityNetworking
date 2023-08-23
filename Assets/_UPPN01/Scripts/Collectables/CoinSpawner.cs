using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CoinSpawner : NetworkBehaviour
{
    [SerializeField]
    private RespawningCoin coinPrefab;
    [SerializeField]
    private int maxCoins = 50;
    [SerializeField]
    private int coinValue = 10;
    [SerializeField]
    private Vector2 xSpawnRange;
    [SerializeField]
    private Vector2 ySpawnRange;
    [SerializeField]
    private LayerMask layerMask;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsServer)
        {
            return;
        }
        for (int i = 0; i < maxCoins; i++)
        {
            SpawnCoin();
        }
    }
    private void SpawnCoin()
    {
        if (!IsServer)
        {
            return;
        }
        RespawningCoin cointInstance = Instantiate(coinPrefab, Vector3.zero, Quaternion.identity);
        cointInstance.SetValue(coinValue);
        cointInstance.GetComponent<NetworkObject>().Spawn();
        cointInstance.transform.position = GetSpawnPoint();
        cointInstance.OnCollect += HandleCoinCollected;
    }

    private void HandleCoinCollected(RespawningCoin obj)
    {
        obj.transform.position = GetSpawnPoint();
        
        obj.Reset();
    }

    private Vector2 GetSpawnPoint()
    {
        float x = Random.Range(xSpawnRange.x, xSpawnRange.y);
        float y = Random.Range(ySpawnRange.x, ySpawnRange.y);
      
        Vector3 result = ((Vector3.right * x) + (Vector3.forward * y));
       
        return result + Vector3.up ;
    }
}
