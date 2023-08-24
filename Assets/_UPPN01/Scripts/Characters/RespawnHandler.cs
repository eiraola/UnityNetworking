using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RespawnHandler : NetworkBehaviour
{
    [SerializeField]
    private NetworkObject playerPrefab;
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
        Destroy(player.gameObject);
        StartCoroutine(RespawnPLayer(player.OwnerClientId));
    }
    private IEnumerator RespawnPLayer(ulong ownerClientId)
    {
        yield return null;
       NetworkObject playerInstance = Instantiate(playerPrefab, SpawnPoint.GetRandomSpawnPos(), Quaternion.identity);
        playerInstance.SpawnAsPlayerObject(ownerClientId);
    }



}
