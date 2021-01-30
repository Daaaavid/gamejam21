using Outlines;
using UnityEngine;

namespace Interaction
{
    [CreateAssetMenu(fileName = "InteractionBus", menuName = "Interaction", order = 0)]
    public class InteractionBus : SystemBusWithValue<InteractableObject, InteractableObjectEvent>
    {
        public bool IsNear()
        {
            return true;
        }
    }
}