using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbyList : MonoBehaviour
{
    [SerializeField]
    private LobbyItem lobbyItemprefab;
    [SerializeField]
    private Transform lobbyObjectParent;
    private bool isJoining = false;
    private bool isRefreshing = false;
    private void OnEnable()
    {
        RefreshListAsync();
    }

    public async void RefreshListAsync()
    {
        if (isRefreshing)
        {
            return;
        }
        isRefreshing = true;
        QueryLobbiesOptions queryOptions = new QueryLobbiesOptions();
        queryOptions.Count = 25;
        queryOptions.Filters = new List<QueryFilter>() { 
        new QueryFilter(
            field: QueryFilter.FieldOptions.AvailableSlots,
            op: QueryFilter.OpOptions.GT,
            value:"0"
            ),
        new QueryFilter(
            field: QueryFilter.FieldOptions.IsLocked,
            op: QueryFilter.OpOptions.EQ,
            value:"0"
            )
        };
        QueryResponse lobbiesResponse = await Lobbies.Instance.QueryLobbiesAsync(queryOptions);
        foreach (Transform child in lobbyObjectParent)
        {
            Destroy(child.gameObject);
        }
        foreach (Lobby lobby in lobbiesResponse.Results)
        {
          LobbyItem lobbyItem = Instantiate(lobbyItemprefab, lobbyObjectParent);
          lobbyItem.Initialize(this, lobby); 
        }
        isRefreshing = false;
    }

    public async void JoinLobbyAsync(Lobby lobby)
    {
        if (isJoining)
        {
            return;
        }
        isJoining = false;
        try
        {
            Lobby joiningLobby = await Lobbies.Instance.JoinLobbyByIdAsync(lobby.Id);
            string joinCode = joiningLobby.Data["JoinOptions"].Value;
            await ClientSingleton.Instance.GameManager.StartClientAsync(joinCode);
        }
        catch (Exception e )
        {
            isJoining = false;
            Debug.LogError(e);
            return;
        }
        isJoining = false;
    }
}
