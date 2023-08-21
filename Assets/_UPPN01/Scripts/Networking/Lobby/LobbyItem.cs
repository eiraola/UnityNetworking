using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbyItem : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI nameTxt;
    [SerializeField]
    private TextMeshProUGUI nPlayersTxt;
    private LobbyList _lobbylist;
    private Lobby _lobby;

    public void Initialize(LobbyList lobbyList, Lobby lobby)
    {
        _lobby = lobby;
        _lobbylist = lobbyList;
        nameTxt.text = lobby.Name;
        nPlayersTxt.text = $"{lobby.Players.Count} / {lobby.MaxPlayers}";
    }

    public void Join()
    {
        _lobbylist.JoinLobbyAsync(_lobby);
    }

}
