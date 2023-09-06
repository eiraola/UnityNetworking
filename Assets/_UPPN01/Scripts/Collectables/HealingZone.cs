using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HealingZone : NetworkBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private Image healPowerBar;
    [SerializeField]
    private int maxHealPower = 30;
    [SerializeField]
    private float healCooldown = 60f;
    [SerializeField]
    private float healTickRate = 1f;
    [SerializeField]
    private int coinsPerTick = 10;
    [SerializeField]
    private int healthPerTick = 10;
}
