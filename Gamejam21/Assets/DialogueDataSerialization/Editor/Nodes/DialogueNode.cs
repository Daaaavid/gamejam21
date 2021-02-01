using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor.Experimental.GraphView;

public class DialogueNode : Node
{
    public string GUID;
    public string dialogueText;
    public bool entryPoint;
    public int type = 0; // 0 = end, 1 = NPC option, 2 = player option, 3 = splitter
    public bool succes = false;
}

