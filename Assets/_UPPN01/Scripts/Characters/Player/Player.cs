using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Cinemachine;
using Unity.Collections;

public class Player : NetworkBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera virtualCamera;
    [SerializeField]
    private int cameraPrioriy = 11;
    [SerializeField]
    private PlayerNameDisplay playerNameDisplay;


    public NetworkVariable<FixedString32Bytes> PlayerName = new NetworkVariable<FixedString32Bytes>();
    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            UserData userData = HostSingleton.Instance.GameManager.NetworkServer.GetUserData(OwnerClientId);
            PlayerName.Value = userData.userName;
        }
        if (IsOwner)
        {
            virtualCamera.Priority = cameraPrioriy;
            playerNameDisplay.Initialize(this);
        }
    }
    public override void OnNetworkDespawn()
    {
        if (IsOwner)
        {
            playerNameDisplay.ShutDown();
        }
    }
}
