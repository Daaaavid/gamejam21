using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HighlightTriggerEvent : UnityEvent<HighlightTrigger>
{
}

public class HighlightTrigger : MonoBehaviour
{
    public HighlightBus HighlightGroup;

    public float EnabledDuration;
    
    public void Activate()
    {
        HighlightGroup.Activate(this);
    }

    public void Deactivate()
    {
        HighlightGroup.Deactivate(this);
    }
}
