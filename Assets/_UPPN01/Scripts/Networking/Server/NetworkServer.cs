using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkServer {
    private NetworkManager _networkManager;
    private Dictionary<ulong, string> _ClientIds = new Dictionary<ulong, string>();
    private Dictionary <string, UserData> _userDatas= new Dictionary<string, UserData>();

    public NetworkServer(NetworkManager networkManager)
    {
        _networkManager = networkManager;
        networkManager.ConnectionApprovalCallback += ApprovalCheck;
        networkManager.OnServerStarted += OnNetworkReady;
    }

    private void OnNetworkReady()
    {
        _networkManager.OnClientDisconnectCallback += OnClientDisconect;
    }

    private void OnClientDisconect(ulong clientID)
    {
        if (_ClientIds.TryGetValue(clientID, out string authID))
        {
            _ClientIds.Remove(clientID);
            _userDatas.Remove(authID);
        }
    }

    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        string payload = System.Text.Encoding.UTF8.GetString(request.Payload);
        UserData userData = JsonUtility.FromJson<UserData>(payload);
        _ClientIds.Add(request.ClientNetworkId, userData.userAuthId);
        _userDatas.Add(userData.userAuthId, userData);
        response.Approved = true;
        response.CreatePlayerObject = true;
    }
}