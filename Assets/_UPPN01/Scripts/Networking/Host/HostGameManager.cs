using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class HostGameManager
{
    private const int maxConections = 20;
    private Allocation allocation;
    private string JoinCode;
    public async Task InitAsync()
    {
        // authenticate player
    }



    public async Task StartHostAsync()
    {
        try
        {
           allocation = await Relay.Instance.CreateAllocationAsync(maxConections);
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }
        try
        {
            JoinCode = await Relay.Instance.GetJoinCodeAsync(allocation.AllocationId);
            Debug.Log(JoinCode);
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }
        UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        RelayServerData relayServerData = new RelayServerData(allocation, "udp");
        transport.SetRelayServerData(relayServerData);
        NetworkManager.Singleton.StartHost();
        NetworkManager.Singleton.SceneManager.LoadScene("Game", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
