using UnityEngine;
using System.Collections.Generic;

public class dialogueNode //stores the text and next nodes in the graph
{
    public string text; //dialogue content
    public string[] options; //connected nodes and dialogue options

    public dialogueNode(string t, string[] o) //constructor
    {
        text = t;
        options = o;
    }
}
