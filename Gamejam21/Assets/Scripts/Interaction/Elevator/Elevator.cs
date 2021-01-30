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

    private void Awake()
    {
        MovementBus.OnProximity.AddListener(CloseElevator);
    }

    public void OpenElevator() {
        leftDoor.Open();
        rightDoor.Open();
    }

    public void CloseElevator() {
        leftDoor.Close();
        rightDoor.Close();
    }

}
