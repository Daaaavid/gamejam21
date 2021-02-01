using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interaction;

public class ComputerScreen : MonoBehaviour
{
    [SerializeField] private InteractableObject myObject;
    public void TurnOnOrOff() {
        gameObject.SetActive(!gameObject.activeSelf);
        if (gameObject.activeSelf) {
            myObject.SplitValue = 1;
        } else {
            myObject.SplitValue = 0;
        }
    }
}
