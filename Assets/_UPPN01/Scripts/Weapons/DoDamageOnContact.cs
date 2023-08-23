using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DoDamageOnContact : MonoBehaviour
{
    [SerializeField]
    private int damage = 5;
    private ulong clientID = 0;
    public void SetOwner(ulong clientID)
    {
        this.clientID = clientID;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<NetworkObject>(out NetworkObject obj))
        {
            if (obj.OwnerClientId == clientID)
            {
                return;
            }
        }
        if (other.TryGetComponent<Health>(out Health health))
        {
            health.TakeDamage(damage);
        }
    }
}
