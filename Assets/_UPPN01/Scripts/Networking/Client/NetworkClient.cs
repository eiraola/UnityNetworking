using System.Collections;
using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkClient: IDisposable
{
    private NetworkManager _networkManager;
    private const string MenuSceneName = "Menu";
    public NetworkClient(NetworkManager networkManager)
    {
        _networkManager = networkManager;
        _networkManager.OnClientDisconnectCallback += OnClientDisconect;
    }
    private void OnClientDisconect(ulong clientID)
    {
        if (clientID!=0 && clientID != _networkManager.LocalClientId)
        {
            return;
        }
        if (SceneManager.GetActiveScene().name != MenuSceneName)
        {
            SceneManager.LoadScene(MenuSceneName);
            _networkManager.Shutdown();
        }
        if (_networkManager.IsConnectedClient)
        {
            _networkManager.Shutdown();
        }
    }

    public void Dispose()
    {
        if (_networkManager!= null)
        {
            _networkManager.OnClientDisconnectCallback -= OnClientDisconect;
        }
    }
}
