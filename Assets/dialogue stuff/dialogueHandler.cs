using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.IO;

public class dialogueHandler : MonoBehaviour
{
    [SerializeField] private Coroutine dialogueCoroutine;
    [SerializeField] private string currentText;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private GameObject dialogueUI;
    [SerializeField] private GameObject[] buttons;
    [SerializeField] private dialogueNode currentNode;
    [SerializeField] private bool runningDialogue = false;
    [SerializeField] private bool optionsGiven = false;
    [SerializeField] private InputAction dialogueInput;
    [SerializeField] private Dictionary<string, dialogueNode> allNodes;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        setUpNodes();
        currentNode = allNodes["1"];
        activateDialogue();
    }

    void setUpNodes()
    {
        allNodes = new Dictionary<string, dialogueNode>();
        string path = Application.dataPath + "/dialogue stuff/dialogue.csv";
        string nodesCSV = File.ReadAllText(path);
        string[] lines = nodesCSV.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

        foreach (string line in lines)
        {
            string[] values = line.Split(" - "); //splits the columns
            string[] options = values[2].Split(" -- ");

            allNodes.Add(values[0], new dialogueNode(values[0], values[1], options));
        }
    }

    void OnEnable()
    {
        dialogueInput.performed += checkCurrentState;
        dialogueInput.Enable();
    }

    void OnDisable()
    {
        dialogueInput.performed -= checkCurrentState;
        dialogueInput.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void checkCurrentState(InputAction.CallbackContext callback)
    {
        if (!runningDialogue)
        {
            hideText();
        }
        else if (runningDialogue && !optionsGiven)
        {
            skipDialogue();
        }
    }

    public void skipDialogue()
    {
        runningDialogue = false;
        StopCoroutine(dialogueCoroutine);
        dialogueText.SetText(currentText);
        if (currentNode.options.Length > 1)
        {
            showOptions();
        }
    }

    public void activateDialogue()
    {
        dialogueUI.SetActive(true);
        currentText = currentNode.text;
        dialogueCoroutine = StartCoroutine(startDialogueLoop());
    }

    IEnumerator startDialogueLoop()
    {
        runningDialogue = true;
        string tempText = "";
        for (int i = 0; i < currentText.Length; i++)
        {
            tempText += currentText[i];
            dialogueText.SetText(tempText);
            yield return new WaitForSeconds(0.05f);
        }
        if (currentNode.options.Length > 1)
        {
            showOptions();
        }
        yield return new WaitForSeconds(0.5f);
        runningDialogue = false;
    }

    void showOptions()
    {
        optionsGiven = true;
        for (int i = 0; i < currentNode.options.Length; i++)
        {
            GameObject b = buttons[i];
            b.GetComponentInChildren<TMP_Text>().SetText(currentNode.options[i]);
            b.SetActive(true);
        }
    }

    void hideOptions()
    {
        optionsGiven = false;
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].SetActive(false);
        }
    }

    public void hideText()
    {
        dialogueText.SetText("");
        currentText = "";
        dialogueUI.SetActive(false);
    }

    public void returnDialogue(TMP_Text t)
    {
        hideOptions();
        currentNode = allNodes[t.text];
        activateDialogue();
        Debug.Log(t.text);
    }
}
