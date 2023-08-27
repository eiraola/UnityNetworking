using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RespawnHandler : NetworkBehaviour
{
    [SerializeField]
    private Player playerPrefab;
    [SerializeField]
    private float keepCoinsPercentage;
    private int deathPlayerCoins = 0;
    public override void OnNetworkSpawn()
    {
        if (!IsServer)
        {
            return;
        }
        Player[] players = GameObject.FindObjectsOfType<Player>();
        foreach (Player player in players)
        {
            HandlePlayerSpawned(player);
        }
        Player.OnPlayerDespawned += HandlePlayerDespwaned;
        Player.OnPlayerSpawned += HandlePlayerSpawned;
    }
    public override void OnNetworkDespawn()
    {
        if (!IsServer)
        {
            return;
        }
        Player.OnPlayerDespawned -= HandlePlayerDespwaned;
        Player.OnPlayerSpawned -= HandlePlayerSpawned;
    }
    private void HandlePlayerSpawned(Player player)
    {
        player.HealthComponent.OnDied += (health) =>HandlePlayerDie(player);
    }
    private void HandlePlayerDespwaned(Player player)
    {
        player.HealthComponent.OnDied -= (health) => HandlePlayerDie(player);
    }
    private void HandlePlayerDie(Player player)
    {
        deathPlayerCoins = (int)(((float) player.CoinWalletComponent.totalCoins.Value) * (keepCoinsPercentage / 100.0f));
        Destroy(player.gameObject);
        StartCoroutine(RespawnPLayer(player.OwnerClientId, deathPlayerCoins));
    }
    private IEnumerator RespawnPLayer(ulong ownerClientId, int currentCoins)
    {
        yield return null;
        Player playerInstance = Instantiate(playerPrefab, SpawnPoint.GetRandomSpawnPos(), Quaternion.identity);
       
        playerInstance.NetworkObject.SpawnAsPlayerObject(ownerClientId);
        playerInstance.CoinWalletComponent.totalCoins.Value += currentCoins;
    }



}
