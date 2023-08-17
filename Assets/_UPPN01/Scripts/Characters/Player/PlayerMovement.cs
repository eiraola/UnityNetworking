using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class PlayerMovement : NetworkBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private InputReader inputReader;
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private float turnSpeed;
    private Vector2 previousMovementInput = Vector2.zero;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            return;
        }
        if (!inputReader)
        {
            return;
        }
        inputReader.MovementEvent += HandleMovement;

    }
    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        if (!IsOwner)
        {
            return;
        }
        if (!inputReader)
        {
            return;
        }
        inputReader.MovementEvent -= HandleMovement;
    }
    private void Update()
    {
        if (!IsOwner)
        {
            return;
        }
        float yRotation = previousMovementInput.x * turnSpeed * Time.deltaTime;
        transform.Rotate(0.0f, yRotation, 0.0f);
    }
    public void FixedUpdate()
    {
        if (!IsOwner)
        {
            return;
        }
        rb.MovePosition(transform.position + transform.forward * previousMovementInput.y * movementSpeed * Time.fixedDeltaTime);
        
    }
    
    public void HandleMovement(Vector2 movement)
    {
        previousMovementInput = movement;
    }
}
