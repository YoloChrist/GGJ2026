using UnityEngine;

public class npcDialogueData : MonoBehaviour
{
    [SerializeField] private string dialogueKey = "1";
    [SerializeField] private bool beingSpokenTo = false;
    [SerializeField] private CrowdAgent ca;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void OnEnable()
    {
        dialogueHandler.triggerEndConversation += endNPCConversation;
    }

    void OnDisable()
    {
        dialogueHandler.triggerEndConversation -= endNPCConversation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string getDialogueKey()
    {
        return dialogueKey;
    }

    void endNPCConversation()
    {
        if (beingSpokenTo)
        {
            ca.ResumeAI();
            beingSpokenTo = false; 
        }
        
    }

    public void startNPCConversation()
    {
        beingSpokenTo = true;
        ca.PauseAI();
    }

    public bool getSpokenTo()
    {
        return beingSpokenTo;
    }
}
