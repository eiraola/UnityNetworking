using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Lobbies;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using Unity.Services.Lobbies.Models;
using System.Text;
using Unity.Services.Authentication;

public class HostGameManager: IDisposable
{
    
    private const int maxConections = 20;
    private Allocation allocation;
    private string JoinCode;
    private string lobbyID;
    private int timeBetweenPings = 15;
    public NetworkServer NetworkServer { get; private set; }
    private Coroutine coHeartBeat = null;

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
        try
        {
            CreateLobbyOptions lobbyOptions = new CreateLobbyOptions();
            lobbyOptions.IsPrivate = false;
            lobbyOptions.Data = new Dictionary<string, DataObject>()
            {
                {"JoinOptions", new DataObject(
                    visibility: DataObject.VisibilityOptions.Member,
                    value: JoinCode
                    )
                }
            };
           string playerName = PlayerPrefs.GetString(NameSelector.PlayerNameKey, "Missing Name");
           Lobby lobby = await Lobbies.Instance.CreateLobbyAsync($"{playerName}", maxConections, lobbyOptions);
           lobbyID = lobby.Id;
            coHeartBeat =  HostSingleton.Instance.StartCoroutine(PingLobby(lobbyID));
        }
        catch (System.Exception e)
        {

            Debug.LogError(e);
            return;
        }
        NetworkServer = new NetworkServer(NetworkManager.Singleton);
        UserData userData = new UserData
        {
            userName = PlayerPrefs.GetString(NameSelector.PlayerNameKey, "Missing Name"),
            userAuthId = AuthenticationService.Instance.PlayerId
        };
        string payload = JsonUtility.ToJson(userData);
        byte[] payloadBytes = Encoding.UTF8.GetBytes(payload);
        NetworkManager.Singleton.NetworkConfig.ConnectionData = payloadBytes;
        NetworkManager.Singleton.StartHost();
        NetworkManager.Singleton.SceneManager.LoadScene("Game", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
    public IEnumerator PingLobby(string lobbyID)
    {
        WaitForSeconds delay = new WaitForSeconds(timeBetweenPings);
        while (true)
        {
            Lobbies.Instance.SendHeartbeatPingAsync(lobbyID);
            yield return delay;
        }
    }

    public async void Dispose()
    {
        HostSingleton.Instance.StopCoroutine(coHeartBeat);
        coHeartBeat = null;
        if (!string.IsNullOrEmpty(lobbyID))
        {
            try
            {
                await Lobbies.Instance.DeleteLobbyAsync(lobbyID);
            }
            catch (LobbyServiceException e)
            {
                Debug.LogError(e);
            }
            lobbyID = string.Empty;
        }
        NetworkServer?.Dispose();
    }
}
