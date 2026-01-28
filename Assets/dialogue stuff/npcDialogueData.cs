using UnityEngine;

public class npcDialogueData : MonoBehaviour
{
    [SerializeField] private string dialogueKey = "1";
    [SerializeField] private string secondDialogueKey = "A";
    [SerializeField] private bool beingSpokenTo = false;
    [SerializeField] private bool CANBeSpokenTo = true;
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
        if (CANBeSpokenTo)
        {
            return dialogueKey;
        }
        else
        {
            return secondDialogueKey;
        }
        
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
        CANBeSpokenTo = false;
    }

    public bool getSpokenTo()
    {
        return beingSpokenTo;
    }
}
