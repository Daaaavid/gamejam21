using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] private ElevatorDoor leftDoor;
    [SerializeField] private ElevatorDoor rightDoor;
    public Transform targetPositionInsideElevator;

    public void GoIntoElevator() {
        leftDoor.Open();
        rightDoor.Open();
    }

    public void GoOutOfElevator() {
        leftDoor.Close();
        rightDoor.Close();
    }

}
