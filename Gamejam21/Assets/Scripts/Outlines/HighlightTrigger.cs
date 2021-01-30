using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Outlines
{
    public class HighlightTriggerEvent : UnityEvent<HighlightTrigger>
    {
    }

    public class HighlightTrigger : MonoBehaviour
    {
        public HighlightBus Bus;

        public void Activate()
        {
            Bus.Activate(this);
        }

        public void Deactivate()
        {
            Bus.Deactivate(this);
        }
    }
}