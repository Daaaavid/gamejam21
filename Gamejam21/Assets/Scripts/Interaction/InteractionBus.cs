using Outlines;
using UnityEngine;
using UnityEngine.Events;

namespace Interaction
{
    [CreateAssetMenu(fileName = "InteractionBus", menuName = "Interaction", order = 0)]
    public class InteractionBus : SystemBusWithValue<InteractableObject, InteractableObjectEvent>
    {
        public UnityEvent OnProximity;
        public UnityEvent OnInvokeReturnPlayer;
    }
}