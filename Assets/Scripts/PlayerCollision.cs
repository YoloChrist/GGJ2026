using System;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public static event Action<npcDialogueData> OnNpcContactStarted;
    public static event Action OnNpcContactEnded;
    public static event Action<string, int> OnNpcInteracted;
    public static event Action<npcDialogueData> accuseNPC;

    [SerializeField] private npcDialogueData currentNpc;
    [SerializeField] private PlayerMovement pm;

    private void OnEnable()
    {
        PlayerInputHandler.OnInteractPressed += HandleInteractPressed;
        dialogueHandler.triggerEndConversation += activatePlayerMovement;
    }

    private void OnDisable()
    {
        PlayerInputHandler.OnInteractPressed -= HandleInteractPressed;
        dialogueHandler.triggerEndConversation -= activatePlayerMovement;
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
        if (currentNpc != null)
        {
            if (currentNpc.getSpokenTo())
                return;
        }
        

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
        
        if (currentNpc.getSpokenTo())
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
            OnNpcInteracted?.Invoke(currentNpc.getDialogueKey(), currentNpc.getSpriteIndex());
            //pm = gameObject.GetComponent<PlayerMovement>();
            pm.SetcanMoveFalse();
            currentNpc.startNPCConversation();
        }
    }

    void activatePlayerMovement()
    {
        //pm = gameObject.GetComponent<PlayerMovement>();
        pm.SetcanMoveTrue();
        Debug.Log("RUN");
    }

    public void callAccuse()
    {
        accuseNPC?.Invoke(currentNpc);
    }
}