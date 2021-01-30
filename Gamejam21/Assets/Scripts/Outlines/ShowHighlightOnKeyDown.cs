using System;
using UnityEngine;

namespace Outlines
{
    public class ShowHighlightOnKeyDown : MonoBehaviour
    {
        public KeyCode Key = KeyCode.Tab;
        private void Update()
        {
            if (Input.GetKeyDown(Key))
            {
                GetComponent<HighlightTrigger>().Activate();
            }
            if (Input.GetKeyUp(Key))
            {
                GetComponent<HighlightTrigger>().Deactivate();
            }
        }

      
    }
}