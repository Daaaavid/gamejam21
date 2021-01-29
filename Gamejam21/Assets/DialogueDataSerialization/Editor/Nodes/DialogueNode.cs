using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor.Experimental.GraphView;

public class DialogueNode : Node
{
    public string GUID;
    public string dialogueText;
    public bool entryPoint;
    public int type = 0;
}
