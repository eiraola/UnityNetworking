using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using System.Linq;
public class Leaderboard : NetworkBehaviour
{
    [SerializeField]
    private Transform leaderboardEntityHolder;
    [SerializeField]
    private LeaderboardEntry leaderBoardEntryPrefab;
    private NetworkList<LeaderboardEntryState> leaderBoardEntities;
    private List<LeaderboardEntry> leaderboardEntries = new List<LeaderboardEntry>();
    private int entriesToDisplay = 8;
    private void Awake()
    {
        leaderBoardEntities = new NetworkList<LeaderboardEntryState>();
    }
    public override void OnNetworkSpawn()
    {
        if (IsClient)
        {
            leaderBoardEntities.OnListChanged += HandleLeaderboardEntitiesChanged;
            foreach (LeaderboardEntryState entry in leaderBoardEntities)
            {
                HandleLeaderboardEntitiesChanged(new NetworkListEvent<LeaderboardEntryState>
                {
                    Type = NetworkListEvent<LeaderboardEntryState>.EventType.Add,
                    Value = entry
                });
            }
        }
        if (!IsServer)
        {
            return;
        }
        Player[] players = GameObject.FindObjectsOfType<Player>();
        foreach (Player player in players)
        {
            HandlePlayerSpawned(player);
        }
        Player.OnPlayerSpawned += HandlePlayerSpawned;
        Player.OnPlayerDespawned += HandlePLayerDespawned;
    }
    public override void OnNetworkDespawn()
    {
        if (IsClient)
        {
            leaderBoardEntities.OnListChanged -= HandleLeaderboardEntitiesChanged;
            
        }
        if (!IsServer)
        {
            return;
        } 
        Player.OnPlayerSpawned -= HandlePlayerSpawned;
        Player.OnPlayerDespawned -= HandlePLayerDespawned;
    }

    private void HandleLeaderboardEntitiesChanged(NetworkListEvent<LeaderboardEntryState> changeEvent)
    {
        switch (changeEvent.Type)
        {
            case NetworkListEvent<LeaderboardEntryState>.EventType.Add:
                EntryAdded(changeEvent);
                break;
            case NetworkListEvent<LeaderboardEntryState>.EventType.Insert:
                break;
            case NetworkListEvent<LeaderboardEntryState>.EventType.Remove:
                EntryRemoved(changeEvent);
                break;
            case NetworkListEvent<LeaderboardEntryState>.EventType.RemoveAt:
                break;
            case NetworkListEvent<LeaderboardEntryState>.EventType.Value:
                EntryUpdated(changeEvent);
                break;
            case NetworkListEvent<LeaderboardEntryState>.EventType.Clear:
                break;
            case NetworkListEvent<LeaderboardEntryState>.EventType.Full:
                break;
            default:
                break;
        }
    }
    private void EntryAdded(NetworkListEvent<LeaderboardEntryState> changeEvent)
    {
        if (leaderboardEntries.Any(x => x.ClienId == changeEvent.Value.ClientId))
        {
            return;
        }
        LeaderboardEntry entry = Instantiate(leaderBoardEntryPrefab, leaderboardEntityHolder);
        entry.Initialize(changeEvent.Value);
        leaderboardEntries.Add(entry);
    }
    private void EntryRemoved(NetworkListEvent<LeaderboardEntryState> changeEvent)
    {
        LeaderboardEntry entryToRemove = leaderboardEntries.FirstOrDefault(x => x.ClienId == changeEvent.Value.ClientId);
        if (entryToRemove != null)
        {
            entryToRemove.transform.parent = null;
            leaderboardEntries.Remove(entryToRemove);
            Destroy(entryToRemove.gameObject);
        }
    }
    private void EntryUpdated(NetworkListEvent<LeaderboardEntryState> changeEvent)
    {
        LeaderboardEntry entryToUpdate = leaderboardEntries.FirstOrDefault(x => x.ClienId == changeEvent.Value.ClientId);
        if (entryToUpdate != null)
        {
           
           entryToUpdate.Coins = changeEvent.Value.Coins;
        }
        leaderboardEntries.Sort((x, y) => y.Coins.CompareTo(x.Coins));

        for (int i = 0; i < leaderboardEntries.Count; i++)
        {
            leaderboardEntries[i].transform.SetSiblingIndex(i);
            leaderboardEntries[i].UpdateText();
        }
    }
    private void HandlePlayerSpawned(Player player)
    {
        leaderBoardEntities.Add(new LeaderboardEntryState { 
        ClientId = player.OwnerClientId,
        PlayerName = player.PlayerName.Value,
        Coins = 0
        
        });;
        player.CoinWalletComponent.totalCoins.OnValueChanged += (oldcoins, newcoins)=> HandleCoinsChanged(player.OwnerClientId, newcoins);
    }
    private void HandlePLayerDespawned(Player player)
    {
        foreach (LeaderboardEntryState entry in leaderBoardEntities)
        {
            if (entry.ClientId != player.OwnerClientId)
            {
                continue;
            }
            leaderBoardEntities.Remove(entry);
            break;
        }
        player.CoinWalletComponent.totalCoins.OnValueChanged -= (oldcoins, newcoins) => HandleCoinsChanged(player.OwnerClientId, newcoins);
    }
    private void HandleCoinsChanged(ulong clientId, int newCoins)
    {
        for (int i = 0; i < leaderBoardEntities.Count; i++)
        {
            if (leaderBoardEntities[i].ClientId != clientId)
            {
                continue;
            }
            leaderBoardEntities[i] = new LeaderboardEntryState
            {
                ClientId = leaderBoardEntities[i].ClientId,
                PlayerName = leaderBoardEntities[i].PlayerName,
                Coins = newCoins
            };
            return;
        }
    }
}
