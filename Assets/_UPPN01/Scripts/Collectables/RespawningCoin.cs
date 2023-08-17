using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class RespawningCoin : Coin
{
    public event Action<RespawningCoin> OnCollect;
    public override int Collect()
    {
        if (!IsServer)
        {
            Show(false);
            return 0;
        }
        if (alreadyCollected)
        {
            return 0;
        }
        alreadyCollected = true;
        Show(false);
        OnCollect?.Invoke(this);
        return coinValue;
    }
    public void Reset()
    {
        alreadyCollected = false;
        Show(true);
    }
}
