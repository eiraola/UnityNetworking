using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using static Input;

[CreateAssetMenu(fileName = "New Input Reader,", menuName = "Input/InputReader")]
public class InputReader : ScriptableObject, IPlayerActions
{
    public event Action<bool> PrimaryFireEvent;
    public event Action<Vector2> MovementEvent;
    private Vector2 mousePos = Vector2.zero;
    private Input input;

    public Vector2 MousePos { get => mousePos; }

    private void OnEnable()
    {
        if (input == null)
        {
            input = new Input();
            input.Player.SetCallbacks(this);
        }
        input.Player.Enable();
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        MovementEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnPrimaryFire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PrimaryFireEvent?.Invoke(true);
            return;
        }
        if (context.canceled)
        {
            PrimaryFireEvent?.Invoke(false);
        }
       
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        mousePos = context.ReadValue<Vector2>();
    }
}
