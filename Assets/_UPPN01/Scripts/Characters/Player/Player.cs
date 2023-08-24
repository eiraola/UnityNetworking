using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Cinemachine;
using Unity.Collections;
using System;

public class Player : NetworkBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera virtualCamera;
    [SerializeField]
    private int cameraPrioriy = 11;
    [SerializeField]
    private PlayerNameDisplay playerNameDisplay;
    [field: SerializeField]
    public Health HealthComponent { get; private set; }
    public static event Action<Player> OnPlayerSpawned;
    public static event Action<Player> OnPlayerDespawned;

    public NetworkVariable<FixedString32Bytes> PlayerName = new NetworkVariable<FixedString32Bytes>();
    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            UserData userData = HostSingleton.Instance.GameManager.NetworkServer.GetUserData(OwnerClientId);
            PlayerName.Value = userData.userName;
            OnPlayerSpawned?.Invoke(this);
        }
        if (IsOwner)
        {
            virtualCamera.Priority = cameraPrioriy;
            playerNameDisplay.Initialize(this);
        }
    }
    public override void OnNetworkDespawn()
    {
        if (IsServer)
        {
            OnPlayerDespawned?.Invoke(this);
        }
        if (IsOwner)
        {
            playerNameDisplay.ShutDown();
        }
    }
}
