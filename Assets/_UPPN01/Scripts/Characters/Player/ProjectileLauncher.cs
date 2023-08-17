using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;
using System;

public class ProjectileLauncher : NetworkBehaviour
{
    [SerializeField]
    private GameObject serverProjectile;
    [SerializeField]
    private GameObject clientProjectile;
    [SerializeField]
    private Transform spawnPoint;
    [SerializeField]
    private InputReader inputReader;
    [SerializeField]
    private ParticleSystem muzzle;
    [SerializeField]
    private Collider PlayerCollider;
    [SerializeField]
    private float fireRate = 1.0f;
    [SerializeField]
    private float projectileSpeed;
    [SerializeField]
    private int costToFire = 1;
    [SerializeField]
    private CoinWallet coinWallet;
    private bool shouldFire = false;
    private float previousFireTime = 0.0f;
    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            return;
        }
        inputReader.PrimaryFireEvent += HandlePrimaryFire;
    }



    public override void OnNetworkDespawn()
    {
        if (!IsOwner)
        {
            return;
        }
        inputReader.PrimaryFireEvent -= HandlePrimaryFire;
    }
    private void HandlePrimaryFire(bool shouldFire)
    {
        this.shouldFire = shouldFire;
    }
    private void Update()
    {
        if (!IsOwner)
        {
            return;
        }
        if (!shouldFire)
        {
            return;
        }
        if (previousFireTime + fireRate > Time.time)
        {
            return;
        }
        previousFireTime = Time.time;
        SpawnDummyProjectile(spawnPoint.position, spawnPoint.up);
       
    }

    private void SpawnDummyProjectile(Vector3 position, Vector3 direction)
    {
        if (!coinWallet.SpendCoins(costToFire))
        {
            return;
        }
        muzzle.Play();
        GameObject projectileInstance = Instantiate(clientProjectile, position,Quaternion.identity);
        projectileInstance.transform.forward = direction;
        Physics.IgnoreCollision(PlayerCollider, projectileInstance.GetComponent<Collider>());
        if (projectileInstance.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.velocity = projectileInstance.transform.forward * projectileSpeed;
        }
        if (IsOwner)
        {
            PrimaryFireServerRpc(spawnPoint.position, spawnPoint.up);
        }
      
    }
    [ServerRpc]
    private void PrimaryFireServerRpc(Vector3 spawnPos, Vector3 direction)
    {
        GameObject projectileInstance = Instantiate(serverProjectile, spawnPos, Quaternion.identity);
        projectileInstance.transform.forward = direction;
        Physics.IgnoreCollision(PlayerCollider, projectileInstance.GetComponent<Collider>());
        if (projectileInstance.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.velocity = projectileInstance.transform.forward * projectileSpeed;
        }
        if (projectileInstance.TryGetComponent<DoDamageOnContact>(out DoDamageOnContact damager))
        {
            damager.SetOwner(OwnerClientId);
        }
        PrimaryFireClientRpc(spawnPos, direction);
    }
    [ClientRpc]
    private void PrimaryFireClientRpc(Vector3 spawnPos, Vector3 direction)
    {
        if (IsOwner)
        {
            return;
        }
        SpawnDummyProjectile(spawnPos, direction);
    }
}
