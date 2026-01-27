using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private CrowdAgent currentNpc;

    private void OnEnable()
    {
        PlayerInputHandler.OnInteractPressed += HandleInteractPressed;
    }

    private void OnDisable()
    {
        PlayerInputHandler.OnInteractPressed -= HandleInteractPressed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        TrySetNpcContact(collision.collider);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        TryClearNpcContact(collision.collider);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        TrySetNpcContact(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        TryClearNpcContact(other);
    }

    private void TrySetNpcContact(Collider2D other)
    {
        Debug.Log($"Player contacted object: {other.name}");
        if (other != null && other.TryGetComponent<CrowdAgent>(out var agent))
        {
            currentNpc = agent;

            // stuff happens
            // maybe show UI prompt
            Debug.Log($"Player in contact with NPC: {agent.name}");
        }
    }

    private void TryClearNpcContact(Collider2D other)
    {
        Debug.Log($"Player ended contact with object: {other.name}");
        if (currentNpc == null || other == null)
            return;

        if (other.TryGetComponent<CrowdAgent>(out var agent) && agent == currentNpc)
        {
            // contact ended - hide prompt
            currentNpc = null;
        }
    }

    private void HandleInteractPressed()
    {
        if (currentNpc != null)
        {
            // do something - open dialogue or kill them or somehting idk
            Debug.Log($"Interacted with NPC: {currentNpc.name}");
        }
    }
}