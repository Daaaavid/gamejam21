using System;
using System.Collections;
using System.Collections.Generic;
using Interaction;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] private ElevatorDoor leftDoor;
    [SerializeField] private ElevatorDoor rightDoor;
    public Transform targetPositionInsideElevator;

    public InteractionBus MovementBus;

    private void Awake()
    {
        MovementBus.OnProximity.AddListener(GoOutOfElevator);
    }

    public void GoIntoElevator() {
        leftDoor.Open();
        rightDoor.Open();
    }

    public void GoOutOfElevator() {
        leftDoor.Close();
        rightDoor.Close();
    }

}
