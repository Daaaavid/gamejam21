using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Subtegral.DialogueSystem.DataContainers;
using UnityEngine.SceneManagement;
using System.Linq;

public class DialogueManager : MonoBehaviour {
    [SerializeField] private Text NPCname;
    [SerializeField] private Text NPCdialogue;
    [SerializeField] private Text PlayerDialogue;
    [SerializeField] private DialogueContainer dialogue;
    [SerializeField] private DialogueNodeData currentNode;
    [SerializeField] private int currentSelected;
    [SerializeField] private Transform leftButton;
    [SerializeField] private Transform rightButton;
    [SerializeField] private int nextScene;


    [Tooltip("0 = grumpy, 1 = not grumpy")]
    [Range(0, 1)] public int grumpy;

    private void Start() {
        PlayerDialogue.text = "";
        var narrativeData = dialogue.NodeLinks.First(); //Entrypoint node
        currentNode = NextNode(narrativeData.TargetNodeGuid);
        AddDialogueText(currentNode);
    }

    public void ContinueDialogue() {
        var choices = dialogue.NodeLinks.Where(x => x.BaseNodeGuid == currentNode.Guid);
        currentNode = NextNode(choices.ElementAt(currentSelected).TargetNodeGuid);
        if(currentNode != null)
        AddDialogueText(currentNode);
    }

    private DialogueNodeData NextNode(string narrativeDataGUID) {
        DialogueNodeData node = dialogue.DialogueNodeData.Find(x => x.Guid == narrativeDataGUID);
        while (node.type == 3) {
            var choices = dialogue.NodeLinks.Where(x => x.BaseNodeGuid == node.Guid);
            node = dialogue.DialogueNodeData.Find(x => x.Guid == choices.ElementAt(grumpy).TargetNodeGuid);
        }
        if(node.type == 0) {
            Debug.Log("Succes: " + node.succes);
            if (node.succes)
                SceneManager.LoadScene(nextScene);
            else
                Application.Quit();
            return null;
        }
        return node;
    }

    private void AddDialogueText(DialogueNodeData node) {
        switch (node.type) {
            case 1:
                currentSelected = 0;
                PlayerDialogue.text = "";
                leftButton.gameObject.SetActive(false);
                rightButton.gameObject.SetActive(false);
                StartCoroutine(TypeSentence(node.DialogueText));
                break;
            case 2:
                var choices = dialogue.NodeLinks.Where(x => x.BaseNodeGuid == node.Guid);
                currentSelected = Random.Range(0, choices.Count());
                PlayerDialogue.text = choices.ElementAt(currentSelected).PortName;
                leftButton.gameObject.SetActive(true);
                rightButton.gameObject.SetActive(true);
                break;
        }
    }

    IEnumerator TypeSentence(string sentence) {
        NPCdialogue.text = "";
        foreach (var letter in sentence.ToCharArray()) {
            NPCdialogue.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(0.3f);
        ContinueDialogue();

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
