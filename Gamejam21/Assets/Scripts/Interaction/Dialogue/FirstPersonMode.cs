using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonMode : MonoBehaviour
{
    TwoDimensionalMovement player;
    public Camera firstPersonCamera;
    [SerializeField] ThoughtBubble firstPersonThoughts;
    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<TwoDimensionalMovement>();
    }

    public void EnterFirstPerson() {
        player.SetFirstPersonMode(true);
        firstPersonCamera.gameObject.SetActive(true);
        if (firstPersonThoughts != null)
            firstPersonThoughts.ActiveListener(true);
    }

    public void ExitFirstPerson() {
        Debug.Log("exiting first person");
        firstPersonCamera.gameObject.SetActive(false);
        if (firstPersonCamera.gameObject.activeSelf)
            Debug.LogError("???? I just turned this thing of");
        player.SetFirstPersonMode(false);
        if (firstPersonThoughts != null)
            firstPersonThoughts.ActiveListener(false);
    }
}
