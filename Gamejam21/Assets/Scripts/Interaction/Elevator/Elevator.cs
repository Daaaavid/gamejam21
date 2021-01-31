using System;
using System.Collections;
using System.Collections.Generic;
using Interaction;
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

    [SerializeField] private float WaitTillCloseDelay = 1.5f;
    
    public void OpenElevator(bool gettingIn) {
        leftDoor.Open();
        rightDoor.Open();
        StartCoroutine(WaitTillClose(gettingIn));
    }

    public void CloseElevator(bool gettingIn) {
        leftDoor.Close();
        rightDoor.Close();
        if (gettingIn)
            StartCoroutine(WaitForTransition());
    }

    IEnumerator WaitTillClose(bool gettingIn) {
        yield return new WaitForSeconds(WaitTillCloseDelay);
        CloseElevator(gettingIn);
    }

    IEnumerator WaitForTransition() {
        yield return new WaitForSeconds(1.2f);
        transitionCamera.transform.position = playerCamera.transform.position;
        transitionCamera.gameObject.SetActive(true);
        player.gameObject.SetActive(false);
        player.transform.position = otherElevator.TargetPosition.position;
        transitionCamera.MakeTransition(playerCamera.transform.position,player,otherElevator);
    }
}
