using System;
using UnityEngine;

[Serializable]
public class DialogueNodeData
{
    public string Guid;
    public string DialogueText;
    public Vector2 Position;
    public int type;
    public bool succes;
    public int switchValue = 0;

    void Awake() {
        switchValue = 0;
    }
}
