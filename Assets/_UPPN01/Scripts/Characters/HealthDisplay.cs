using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class HealthDisplay : NetworkBehaviour
{
    [SerializeField]
    private Health health;
    [SerializeField]
    private Image healthDisplay;


    public override void OnNetworkSpawn()
    {
       
        if (!IsClient)
        {
            return;
        }
        if (health)
        {
            health.CurrentHealth.OnValueChanged += HandleHealthChange;
            HandleHealthChange(0, health.MaxHealth);
        }
    }
    public override void OnNetworkDespawn()
    {
        if (!IsClient)
        {
            return;
        }
        if (health)
        {
            health.CurrentHealth.OnValueChanged -= HandleHealthChange;
        }
    }
    public void HandleHealthChange(int oldvalue, int newValue)
    {
        healthDisplay.fillAmount = ((float)newValue / health.MaxHealth);
    }
}
