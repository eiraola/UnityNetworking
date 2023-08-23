using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using System;
public class Health : NetworkBehaviour
{
    [SerializeField]
    private int maxHealth = 100;
    public NetworkVariable<int> CurrentHealth = new NetworkVariable<int>();
    private bool isDead = false;
    public int MaxHealth { get => maxHealth;}
    public Action<Health> OnDied;
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsServer)
        {
            return;
        }
        CurrentHealth.Value = maxHealth;
    }
    public void TakeDamage(int damage)
    {
        ModifyHealth(-damage);
    }
    public void RestoreHealth(int health)
    {
        ModifyHealth(health);
    }

    public void ModifyHealth(int health)
    {
        if (!IsServer)
        {
            return;
        }
        if (isDead)
        {
            return;
        }
        CurrentHealth.Value = Math.Clamp(CurrentHealth.Value + health, 0, maxHealth );
        if (CurrentHealth.Value == 0)
        {
            Die();
        }
    }
    public void Die()
    {

        isDead = true;
        OnDied?.Invoke(this);
    }
}
