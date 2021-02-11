using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interaction;

public class ElevatorButton : MonoBehaviour
{
    public Elevator elevator;
    AudioSource buttons;
    [SerializeField] private AudioClip keycardAccepted;
    [SerializeField] private AudioClip keycardDenied;
    [SerializeField] private FirstPersonMode firstPersonMode;


    private void Start() {
        buttons = GetComponent<AudioSource>();
        firstPersonMode = GetComponent<FirstPersonMode>();
    }

    public void OnButtonPressed(int button) {
        if (GetComponent<InteractableObject>().SplitValue == 0) {
            buttons.clip = keycardDenied;
            buttons.Play();
        } else {
            buttons.clip = keycardAccepted;
            buttons.Play();
        }
        StartCoroutine(CloseCamera());
    }

    void ExitFirstPerson() {
        firstPersonMode.ExitFirstPerson();
        elevator.OpenElevator(true);
    }

    IEnumerator CloseCamera() {
        yield return new WaitForSeconds(1.2f);
        ExitFirstPerson();
    }
}
