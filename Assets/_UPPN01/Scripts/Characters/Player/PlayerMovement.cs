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
    [SerializeField]
    private float maxAngle = 45.0f;
    [SerializeField]
    private float speed = 0.01f;
    private float currentTargetAngle = 0.0f;
    private float previousMousePos = 0.0f;
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
    public void FixedUpdate()
    {
        if (!IsOwner)
        {
            return;
        }
        rb.velocity = (transform.forward * previousMovementInput.y + transform.right * previousMovementInput.x) * movementSpeed;
        MoveCannon();

    }
    
    public void HandleMovement(Vector2 movement)
    {
        previousMovementInput = movement;
    }
    public void MoveCannon()
    {
        currentTargetAngle += (inputReader.MousePos.x - previousMousePos) * turnSpeed;
        previousMousePos = inputReader.MousePos.x;
        transform.rotation = Quaternion.Euler(0.0f, currentTargetAngle, 0.0f);
    }
}
