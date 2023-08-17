using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class Coin : NetworkBehaviour
{
    [SerializeField]
    private MeshRenderer mesh;
    protected int coinValue = 5;
    protected bool alreadyCollected;

    public int CoinValue { get => coinValue;}

    public abstract int Collect();
    public void SetValue(int value)
    {
        coinValue = value;
    }
    protected void Show(bool show)
    {
        mesh.enabled = show;
    }
}
