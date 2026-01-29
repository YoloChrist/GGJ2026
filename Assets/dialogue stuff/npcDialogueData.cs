using UnityEngine;
using System.Linq;
using System;

public class npcDialogueData : MonoBehaviour
{
    [SerializeField] private string dialogueKey = "1";
    [SerializeField] private string secondDialogueKey = "A";
    [SerializeField] private bool beingSpokenTo = false;
    [SerializeField] private bool CANBeSpokenTo = true;
    [SerializeField] private CrowdAgent ca;
    [SerializeField] private GameObject speechIndicator;
    public static event Action addToTimerCounter;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        string temp = "";

        int val = rnjesus.rand.Next(1, 9);

        switch (val)
        {
            case 1:
                temp = "A";
                break;
            case 2:
                temp = "B";
                break;
            case 3:
                temp = "C";
                break;
            case 4:
                temp = "D";
                break;
            case 5:
                temp = "E";
                break;
            case 6:
                temp = "F";
                break;
            case 7:
                temp = "G";
                break;
            case 8:
                temp = "H";
                break;
            case 9:
                temp = "I";
                break;
        }

        if (gameObject.name.Contains("Good"))
        {
            secondDialogueKey = temp;
        }
        else
        {
            dialogueKey = temp;
        }
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
            if (CANBeSpokenTo && gameObject.name.Contains("Good"))
            {
                if (speechIndicator != null)
                {
                    speechIndicator.SetActive(false);
                }
                addToTimerCounter?.Invoke();
            }
            CANBeSpokenTo = false;
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
