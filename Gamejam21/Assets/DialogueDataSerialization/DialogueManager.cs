using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Subtegral.DialogueSystem.DataContainers;
using System.Linq;

public class DialogueManager : MonoBehaviour {
    [SerializeField] private Text NPCname;
    [SerializeField] private Text NPCdialogue;
    [SerializeField] private Text PlayerDialogue;
    [SerializeField] private DialogueContainer dialogue;
    [SerializeField] private DialogueNodeData currentNode;
    [SerializeField] private int currentSelected;


    [Tooltip("0 = grumpy, 1 = not grumpy")]
    [Range(0, 1)] public int grumpy;

    private void Start() {
        NPCname.text = "Harold from IT";
        var narrativeData = dialogue.NodeLinks.First(); //Entrypoint node
        currentNode = NextNode(narrativeData.TargetNodeGuid);
        AddDialogueText(currentNode);
    }

    public void ContinueDialogue() {
        var choices = dialogue.NodeLinks.Where(x => x.BaseNodeGuid == currentNode.Guid);
        currentNode = NextNode(choices.ElementAt(currentSelected).TargetNodeGuid);
        AddDialogueText(currentNode);
    }

    private DialogueNodeData NextNode(string narrativeDataGUID) {
        DialogueNodeData node = dialogue.DialogueNodeData.Find(x => x.Guid == narrativeDataGUID);
        while (node.type == 3) {
            var choices = dialogue.NodeLinks.Where(x => x.BaseNodeGuid == node.Guid);
            node = dialogue.DialogueNodeData.Find(x => x.Guid == choices.ElementAt(grumpy).TargetNodeGuid);
        }
        if(node.type == 0) {
            var narrativeData = dialogue.NodeLinks.First(); //Entrypoint node
            return NextNode(narrativeData.TargetNodeGuid);
        }
        return node;
    }

    private void AddDialogueText(DialogueNodeData node) {
        switch (node.type) {
            case 1:
                currentSelected = 0;
                NPCdialogue.text = node.DialogueText;
                ContinueDialogue();
                break;
            case 2:
                var choices = dialogue.NodeLinks.Where(x => x.BaseNodeGuid == node.Guid);
                currentSelected = Random.Range(0, choices.Count());
                PlayerDialogue.text = choices.ElementAt(currentSelected).PortName;
                break;
        }
    }

    public void NextDialogueOption(int value) {
        if (currentNode.type != 2)
            return;
        currentSelected += value;
        var choices = dialogue.NodeLinks.Where(x => x.BaseNodeGuid == currentNode.Guid);
        if(currentSelected >= choices.Count()) {
            currentSelected = 0;
        }else if(currentSelected < 0) {
            currentSelected = choices.Count() - 1;
        }
        PlayerDialogue.text = choices.ElementAt(currentSelected).PortName;
    }

}
