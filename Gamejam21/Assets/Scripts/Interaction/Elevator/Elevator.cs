using System;
using System.Collections;
using System.Collections.Generic;
using Interaction;
using Subtegral.DialogueSystem.DataContainers;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] private ElevatorDoor leftDoor;
    [SerializeField] private ElevatorDoor rightDoor;
    [SerializeField] private TransistionCamera transitionCamera;
    [SerializeField] private Elevator otherElevator;
    [SerializeField] private Transform TargetPosition;
    [SerializeField] private TwoDimensionalMovement player;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private AudioSource elevatorDoor;
    [SerializeField] public AudioSource elevatorDone;
    [SerializeField] private AudioSource buttons;
    [SerializeField] private float WaitTillCloseDelay = 1.5f;

    [Header("OnExitDialogue")]
    public float OnExitDialogDelay;
    public InteractionBus DialogueBus;
    public DialogueContainer Dialogue;
    public int SplitValue;

    public void OpenElevator(bool gettingIn) {
        if (PlayerPrefs.GetInt("HasKeyCard") == 0) {
            player.interactionComplete = true;
            return;
        }
        elevatorDoor.Play();
        leftDoor.Open();
        rightDoor.Open();
        StartCoroutine(WaitTillClose(gettingIn));
    }

    public void CloseElevator(bool gettingIn) {
        elevatorDoor.Play();
        leftDoor.Close();
        rightDoor.Close();
        if (gettingIn)
        {
            StartCoroutine(WaitForTransition());
        }
        else
        {
            if(Dialogue != null) StartCoroutine(WaitForDelay());
        }
    }

    IEnumerator WaitForDelay()
    {
        yield return new WaitForSeconds(OnExitDialogDelay);
        //var io = new InteractableObject();
        //io.Dialogue = Dialogue;
        //io.SplitValue = SplitValue;
        //DialogueBus.SetValue(io);
    }

    IEnumerator WaitTillClose(bool gettingIn) {
        yield return new WaitForSeconds(0.5f);
        if (!gettingIn) {
            player.interactionComplete = true;
        }
        yield return new WaitForSeconds(WaitTillCloseDelay -0.5f);
        CloseElevator(gettingIn);
    }

    IEnumerator WaitForTransition() {
        yield return new WaitForSeconds(1.2f);
        transitionCamera.transform.position = playerCamera.transform.position;
        transitionCamera.gameObject.SetActive(true);
        player.gameObject.SetActive(false);
        player.transform.position = otherElevator.TargetPosition.position;
        player.playermoddel.rotation = Quaternion.Euler(Vector3.zero);
        transitionCamera.MakeTransition(playerCamera.transform.position,player,otherElevator);
    }
}
