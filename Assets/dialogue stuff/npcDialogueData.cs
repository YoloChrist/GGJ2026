using UnityEngine;
using System.Linq;
using System;

public class npcDialogueData : MonoBehaviour
{
    [SerializeField] private string dialogueKey = "1";
    [SerializeField] private string secondDialogueKey = "A";
    [SerializeField] private int spriteIndex;
    [SerializeField] private int otherSpriteIndex;
    [SerializeField] private bool beingSpokenTo = false;
    [SerializeField] private bool CANBeSpokenTo = true;
    [SerializeField] private CrowdAgent ca;
    [SerializeField] private GameObject speechIndicator;
    [SerializeField] private SpriteRenderer sr;
    public static event Action addToTimerCounter;
    [SerializeField] private npcspriteManager npcsm;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        string temp = "";

        int val = rnjesus.rand.Next(1, 8);

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
                temp = "I";
                break;
        }

        if (gameObject.name.Contains("Good"))
        {
            secondDialogueKey = temp;
            spriteIndex = int.Parse(dialogueKey)-1;
            sr.sprite = npcsm.getSprite(spriteIndex, "special");
        }
        else if (gameObject.name.Contains("Bad"))
        {
            dialogueKey = "H";
            secondDialogueKey = "H";
            spriteIndex = 4;
            sr.sprite = npcsm.getSprite(spriteIndex, "special");
        }
        else
        {
            int i = rnjesus.rand.Next(0,6);
            sr.sprite = npcsm.getSprite(i, "generic");
            dialogueKey = temp;
            secondDialogueKey = temp;
            spriteIndex = rnjesus.rand.Next(0, 3);
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

    public int getSpriteIndex()
    {
        return spriteIndex;
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
