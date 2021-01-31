using System;
using UnityEngine;

namespace Outlines
{
    public class ShowHighlightOnHover : MonoBehaviour
    {
        public HighlightTrigger Trigger;
        private void OnMouseEnter()
        {
            Trigger.Activate();
        }
        private void OnMouseExit()
        {
            Trigger.Deactivate();
        }
    }
}