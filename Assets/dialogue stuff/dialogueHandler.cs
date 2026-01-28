using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.IO;
using System.Linq;

public class dialogueHandler : MonoBehaviour
{
    //Dialogue Graph
    [SerializeField] private dialogueNode currentNode;
    [SerializeField] private Dictionary<string, dialogueNode> allNodes;
    //Dialogue Functions Stuff
    [SerializeField] private Coroutine dialogueCoroutine;
    [SerializeField] private string currentText;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private GameObject[] buttons;
    [SerializeField] private bool runningDialogue = false;
    [SerializeField] private bool optionsGiven = false;
    [SerializeField] private InputAction dialogueInput;
    //Notes Stuff
    [SerializeField] private TMP_Text notesText;
    [SerializeField] private GameObject notesUI;
    [SerializeField] private GameObject saveHintBtn;
    [SerializeField] private bool saveUsed = false;
    [SerializeField] private List<string> savedDialogue = new List<string>();
    //Other
    [SerializeField] private GameObject dialogueUI;

    void Start()
    {
        setUpNodes();
    }

    void setUpNodes() //gets the csv and converts it to dialogueNode classes and adds them to a dictionary
    {
        allNodes = new Dictionary<string, dialogueNode>();
        //getting and reading file
        string path = Application.dataPath + "/dialogue stuff/dialogue.csv";
        string nodesCSV = File.ReadAllText(path);
        string[] lines = nodesCSV.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries); //splits lines

        foreach (string line in lines)
        {
            string[] values = line.Split(" - "); //splits the columns
            //Debug.Log(values[0]);
            string[] options = values[2].Split(" -- "); //splits options

            allNodes.Add(values[0], new dialogueNode(values[1], options)); //dictionary addition
        }
    }

    void OnEnable()
    {
        //event for input
        dialogueInput.performed += checkCurrentState;
        dialogueInput.Enable();
        PlayerInputHandler.OnInteractPressed += startConversation;
    }

    void OnDisable()
    {
        //event for input
        dialogueInput.performed -= checkCurrentState;
        dialogueInput.Disable();
        PlayerInputHandler.OnInteractPressed -= startConversation;
    }

    public void startConversation(string key) //used to set the start node and start dialogue
    {
        saveUsed = false;
        currentNode = allNodes[key]; //sets start node
        if (key.All(char.IsDigit)) //checks if the node is a number, as those ones can give hints
        {
            saveHintBtn.SetActive(true);
        }
        activateDialogue(); //starts dialogue
    }

    void checkCurrentState(InputAction.CallbackContext callback) //this takes an input and decides if something needs to be done
    {
        if (!runningDialogue && !optionsGiven) //if the dialogue is not running and no options are availiable, hide the text and end the conversation
        {
            hideText();
        }
        else if (runningDialogue) //if the dialogue is running, it skips the animations
        {
            skipDialogue();
        }
    }

    public void skipDialogue() //skips the dialogue 'animation'
    {
        runningDialogue = false;
        StopCoroutine(dialogueCoroutine);
        dialogueText.SetText(currentText);
        if (currentNode.options[0] != "End") //if it's not the end, the options are shown
        {
            showOptions();
        }
    }

    void activateDialogue() //starts a new line of dialogue
    {
        dialogueUI.SetActive(true);
        currentText = currentNode.text;
        dialogueCoroutine = StartCoroutine(startDialogueLoop());
    }

    IEnumerator startDialogueLoop() //displays the dialogue one character at a time
    {
        runningDialogue = true;
        string tempText = "";
        for (int i = 0; i < currentText.Length; i++)
        {
            tempText += currentText[i];
            dialogueText.SetText(tempText);
            yield return new WaitForSeconds(0.01f);
        }
        if (currentNode.options[0] != "End") //if it's not the end, the options are shown
        {
            showOptions();
        }
        yield return new WaitForSeconds(0.5f);
        runningDialogue = false;
    }

    void showOptions() //shows the current options
    {
        optionsGiven = true;
        for (int i = 0; i < currentNode.options.Length; i++) //ensures the right amount of buttons are shown
        {
            GameObject b = buttons[i];
            b.GetComponentInChildren<TMP_Text>().SetText(currentNode.options[i]);
            b.SetActive(true);
        }
    }

    void hideOptions() //hides all dialogue options
    {
        optionsGiven = false;
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].SetActive(false);
        }
    }

    public void hideText() //hides all dialogue UI
    {
        dialogueText.SetText("");
        currentText = "";
        saveHintBtn.SetActive(false);
        dialogueUI.SetActive(false);
    }

    public void returnDialogue(TMP_Text t) //gets the clicked button and checks the text to set the next node
    {
        hideOptions();
        currentNode = allNodes[t.text];
        activateDialogue();
        //Debug.Log(t.text);
    }

    public void saveDialogue() //saves the current lines of dialogue, can only be used once with each npc
    {
        if (!saveUsed)
        {
            saveUsed = true;
            savedDialogue.Add(currentNode.text);
            Debug.Log(savedDialogue[savedDialogue.Count-1]);
        }
    }

    public void viewNotes() //shows/hides the notes screen and updates the notes text.
    {
        if (notesUI.activeInHierarchy) //checks if the canvas is enabled
        {
            notesUI.SetActive(false);
        }
        else
        {
            notesUI.SetActive(true);
            string tempstring = "";

            foreach (string note in savedDialogue)
            {
                tempstring = tempstring + "-> " + note + "\n"; //formats the notes
            }

            notesText.SetText(tempstring);
        }
    }
}
