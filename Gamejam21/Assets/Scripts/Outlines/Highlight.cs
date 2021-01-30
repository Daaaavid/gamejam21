using System;
using System.Collections;
using Interaction;
using UnityEngine;

public class Highlight : MonoBehaviour
{
    public HighlightBus Bus;
    public Outline target;

    private void Awake()
    {
        Bus.OnActivate.AddListener(t => ActivateOutline());
        Bus.OnDeactivate.AddListener(t => DeactivateOutline());
    }

    private void ActivateOutline()
    {
        target.enabled = true;
    }
    
    private void DeactivateOutline()
    {
        target.enabled = false;
    }
}