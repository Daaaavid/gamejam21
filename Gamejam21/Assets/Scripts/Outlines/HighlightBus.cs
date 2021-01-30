using System.Collections;
using System.Collections.Generic;
using Outlines;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Highlight", fileName = "highlightBus") ]
public class HighlightBus : ScriptableObject
{
    public readonly HighlightTriggerEvent OnActivate = new HighlightTriggerEvent();
    public readonly HighlightTriggerEvent OnDeactivate = new HighlightTriggerEvent();
    
    public void Activate(HighlightTrigger trigger)
    {
        OnActivate.Invoke(trigger);
    }

    public void Deactivate(HighlightTrigger trigger)
    {
        OnDeactivate.Invoke(trigger);
    }
}
