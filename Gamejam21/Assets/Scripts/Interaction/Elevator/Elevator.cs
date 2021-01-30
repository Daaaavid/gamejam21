using System;
using System.Collections;
using System.Collections.Generic;
using Interaction;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] private ElevatorDoor leftDoor;
    [SerializeField] private ElevatorDoor rightDoor;

    public InteractionBus MovementBus;
    public void OpenElevator() {
        MovementBus.OnProximity.AddListener(CloseElevator);
        leftDoor.Open();
        rightDoor.Open();
    }

    public void CloseElevator() {
        Debug.Log("closing");
        leftDoor.Close();
        rightDoor.Close();
        MovementBus.OnProximity.RemoveListener(CloseElevator);
    }

}
