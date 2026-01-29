using System;
using UnityEngine;
using UnityEngine.InputSystem;

public static class rnjesus
{
    public static readonly System.Random rand = new System.Random();
}

public class PlayerInputHandler : MonoBehaviour
{
    public static event Action<Vector2> OnMoveInput;
    public static event Action OnInteractPressed;

    [SerializeField] private InputAction moveAction;
    [SerializeField] private InputAction interactAction;
    private Vector2 previousInput;

    void Awake()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        interactAction = InputSystem.actions.FindAction("Interact");

        if (moveAction == null)
        {
            Debug.LogError("Move action not found in Input System.", this);
            enabled = false;
            return;
        }

        if (interactAction == null)
            Debug.LogWarning("Interact action not found in Input System. Create an action named 'Interact'.", this);
    }

    void Update()
    {
        ProcessInput();
    }

    private void ProcessInput()
    {
        if (moveAction != null)
        {
            Vector2 input = moveAction.ReadValue<Vector2>();

            // Normalize to prevent faster diagonal movement
            if (input.sqrMagnitude > 1f)
                input.Normalize();

            // Only invoke if input changed
            if (input != previousInput)
            {
                OnMoveInput?.Invoke(input);
                previousInput = input;
            }
        }

        if (interactAction != null && interactAction.WasPressedThisFrame())
            OnInteractPressed?.Invoke();
    }
}