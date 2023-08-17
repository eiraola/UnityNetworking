using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class Lifetime : NetworkBehaviour
{
    [SerializeField]
    private float lifeTime = 1.0f;
    private float timeToDestroy = 0.0f;
    private bool isSpawned = false;
    public  void Start()
    {
        timeToDestroy = lifeTime + Time.time;
        isSpawned = true;
    }
    private void Update()
    {
        if (isSpawned && timeToDestroy < Time.time)
        {
            Destroy(this.gameObject);
        }
    }

}
