using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class PlayerAim : NetworkBehaviour
{
    [SerializeField]
    private InputReader inputReader;
    [SerializeField]
    private Transform  turretTransform;
    [SerializeField]
    private float maxAngle = 45.0f;
    [SerializeField]
    private float speed = 0.01f;
    private float currentTargetAngle = 0.0f;
    private float previousMousePos = 0.0f;
    private void LateUpdate()
    {
        if (!IsOwner)
        {
            return;
        }
        MoveCannon();
    }
    public void MoveCannon()
    {
        currentTargetAngle += (inputReader.MousePos.x - previousMousePos) * speed;
        previousMousePos = inputReader.MousePos.x;
        if (currentTargetAngle > maxAngle)
        {
            currentTargetAngle = maxAngle;
        }
        if (currentTargetAngle < -maxAngle)
        {
            currentTargetAngle = -maxAngle;
        }
        turretTransform.localRotation = Quaternion.Euler(0.0f, currentTargetAngle, 0.0f);
    }
}
