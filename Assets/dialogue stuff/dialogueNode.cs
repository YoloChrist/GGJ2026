using UnityEngine;
using System.Collections.Generic;

public class dialogueNode
{
    public string name;
    public string text;
    public string[] options;
    public dialogueNode[] connectedNodes;

    public dialogueNode(string n, string t, string[] o)
    {
        name = n;
        text = t;
        options = o;
        connectedNodes = new dialogueNode[options.Length];
    }

    public void addNode(dialogueNode d)
    {
        for (int i=0; i<options.Length; i++)
        {
            if (d.name == options[i])
            {
                connectedNodes[i] = d;
            }
        }
    }
}
