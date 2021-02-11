using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartInFirstPerson : MonoBehaviour
{
    FirstPersonMode firstPersonmode;
    // Start is called before the first frame update
    void Start()
    {
        firstPersonmode = GetComponent<FirstPersonMode>();
        firstPersonmode.EnterFirstPerson();
    }
}
