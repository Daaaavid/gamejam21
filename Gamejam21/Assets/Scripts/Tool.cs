using System.Collections;
using System.Collections.Generic;
using Interaction;
using UnityEngine;

namespace DefaultNamespace
{
    [ExecuteInEditMode]
    public class Tool : MonoBehaviour
    {
        public GameObject empty;

        public void DoStuff()
        {
            var a = FindObjectsOfType<AudioListener>();
            
            Debug.Log(a[0].name);
            Debug.Log(a[1].name);
        }
    }
}