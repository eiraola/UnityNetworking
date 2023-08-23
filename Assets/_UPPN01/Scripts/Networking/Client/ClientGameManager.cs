using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Unity.Services.Core;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using System.Text;
using Unity.Services.Authentication;

public class ClientGameManager {

    private JoinAllocation joinAllocation;
    private NetworkClient _networkClient;

    public async Task<bool> InitAsync()
    {
        await UnityServices.InitializeAsync();
        _networkClient = new NetworkClient(NetworkManager.Singleton);
        await AuthenticationWrapper.DoAuth();
        EAuthState authState = await AuthenticationWrapper.DoAuth();
        if (authState == EAuthState.Authenticated)
        {
            return true;
        }
        return false;
    }
    public async Task StartClientAsync(string joinCode)
    {
        try
        {
            joinAllocation = await Relay.Instance.JoinAllocationAsync(joinCode);
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
            return;
        }
        UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        RelayServerData relayServerData = new RelayServerData(joinAllocation, "udp");
        transport.SetRelayServerData(relayServerData);

        UserData userData = new UserData
        {
            userName = PlayerPrefs.GetString(NameSelector.PlayerNameKey, "Missing Name"),
            userAuthId = AuthenticationService.Instance.PlayerId
        };
        string payload = JsonUtility.ToJson(userData);
        byte[] payloadBytes = Encoding.UTF8.GetBytes(payload);
        NetworkManager.Singleton.NetworkConfig.ConnectionData = payloadBytes;
        NetworkManager.Singleton.StartClient();
    }
}
