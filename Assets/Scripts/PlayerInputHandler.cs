using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public static event Action<Vector2> OnMoveInput;

    private InputAction moveAction;
    private Vector2 previousInput;

    void Awake()
    {
        moveAction = InputSystem.actions.FindAction("Move");

        if (moveAction == null)
        {
            Debug.LogError("Move action not found in Input System.", this);
            enabled = false;
        }
    }

    void Update()
    {
        ProcessInput();
    }

    private void ProcessInput()
    {
        if (moveAction == null) return;

        Vector2 input = moveAction.ReadValue<Vector2>();

        // Normalize to prevent faster diagonal movement
        if (input.sqrMagnitude > 1f)
        {
            input.Normalize();
        }

        // Only invoke if input changed
        if (input != previousInput)
        {
            OnMoveInput?.Invoke(input);
            previousInput = input;
        }
    }

    private void OnDestroy()
    {
        OnMoveInput = null;
    }
}