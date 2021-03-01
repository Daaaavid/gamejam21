using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Subtegral.DialogueSystem.DataContainers;
using System.Linq;
using Interaction;

public class ThoughtBubble : MonoBehaviour
{
    public InteractionBus Bus;
    
    [SerializeField] private Text thoughtBubbleText;
    [SerializeField] private Transform thoughtBubble;
    [SerializeField] public DialogueContainer _dialogue;
    [SerializeField] private AudioSource audio;
    [SerializeField] private AudioClip open;
    [SerializeField] private AudioClip close;

    [SerializeField] private DialogueNodeData currentNode;
    [Tooltip("0 = false, 1 = true")]
    [Range(0, 1)] public int _splitterValue;
    [SerializeField] private bool useable = true;
    public void ActiveListener(bool value) {
        if (value) {
            Bus.OnChange.AddListener(o => NewThoughtBubble(o.Dialogue, o.SplitValue));
        } else {
            Bus.RemoveListener(o => NewThoughtBubble(o.Dialogue, o.SplitValue));
        }
    }

    public void NewThoughtBubble(DialogueContainer dialogue, int splittervalue) {
        if(!dialogue ||!useable) return;
        audio.clip = open;
        audio.Play();
        thoughtBubble.gameObject.SetActive(true);
        thoughtBubbleText.text = "";
        _dialogue = dialogue;
        _splitterValue = splittervalue;
        var narrativeData = dialogue.NodeLinks.First(); //Entrypoint node
        currentNode = NextNode(narrativeData.TargetNodeGuid);
        if (currentNode != null)
            AddThoughtText(currentNode);
    }

    private DialogueNodeData NextNode(string narrativeDataGUID) {
        DialogueNodeData node = _dialogue.DialogueNodeData.Find(x => x.Guid == narrativeDataGUID);
        while (node.type == 3 || node.type == 4) {
            var choices = _dialogue.NodeLinks.Where(x => x.BaseNodeGuid == node.Guid);
            DialogueNodeData tempNode;
            if(node.type == 3)
                tempNode = _dialogue.DialogueNodeData.Find(x => x.Guid == choices.ElementAt(node.switchValue).TargetNodeGuid);
            else {
                Debug.Log(node.DialogueText + ": " + PlayerPrefs.GetInt(node.DialogueText));
                Debug.Log(choices.ElementAt(PlayerPrefs.GetInt(node.DialogueText)).PortName);
                tempNode = _dialogue.DialogueNodeData.Find(x => x.Guid == choices.ElementAt(PlayerPrefs.GetInt(node.DialogueText)).TargetNodeGuid);
            } 
            node.switchValue += 1;
            if (node.switchValue > choices.Count() - 1) {
                node.switchValue = 0;
            }
            node = tempNode;
        }
        if (node.type == 0) {
            audio.clip = close;
            audio.Play();
            thoughtBubble.gameObject.SetActive(false);
            return null;
        }
        return node;
    }

    private void AddThoughtText(DialogueNodeData node) {
        switch (node.type) {
            case 1:
                StopAllCoroutines();
                StartCoroutine(TypeSentence(node.DialogueText));
                break;
            case 2:
                Debug.LogError("player dialogue nodes cannot be used in thoughtbubbles.");
                break;
        }
    }

    IEnumerator TypeSentence(string sentence) {
        thoughtBubbleText.text = "";
        foreach (char letter in sentence.ToCharArray()) {
            thoughtBubbleText.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(1f);
        ContinueThoughts();

    }

    void ContinueThoughts() {
        var choices = _dialogue.NodeLinks.Where(x => x.BaseNodeGuid == currentNode.Guid);
        currentNode = NextNode(choices.ElementAt(0).TargetNodeGuid);
        if (currentNode != null)
        AddThoughtText(currentNode);
    }

    public void TurnOff() {
        if (thoughtBubble.gameObject.activeSelf) {
            audio.clip = close;
            audio.Play();
            thoughtBubble.gameObject.SetActive(false);
        }
        ActiveListener(false);
    }
}
