using System;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public static event Action<npcDialogueData> OnNpcContactStarted;
    public static event Action OnNpcContactEnded;
    public static event Action<string> OnNpcInteracted;

    private npcDialogueData currentNpc;

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

    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    TrySetNpcContact(other);
    //}

    //private void OnTriggerExit2D(Collider2D other)
    //{
    //    TryClearNpcContact(other);
    //}

    private void TrySetNpcContact(Collider2D other)
    {
        if (other != null && other.TryGetComponent<npcDialogueData>(out var agent))
        {
            currentNpc = agent;

            OnNpcContactStarted?.Invoke(agent);
        }
    }

    private void TryClearNpcContact(Collider2D other)
    {
        if (currentNpc == null || other == null)
            return;

        if (other.TryGetComponent<npcDialogueData>(out var agent) && agent == currentNpc)
        {
            currentNpc = null;
            OnNpcContactEnded?.Invoke();
        }
    }

    private void HandleInteractPressed()
    {
        if (currentNpc != null)
        {
            OnNpcInteracted?.Invoke(currentNpc.getDialogueKey());
        }
    }
}