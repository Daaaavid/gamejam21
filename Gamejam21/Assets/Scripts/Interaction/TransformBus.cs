using Outlines;
using UnityEngine;

namespace Interaction
{
    [CreateAssetMenu(fileName = "TransformBus", menuName = "TransformBus", order = 0)]
    public class TransformBus : SystemBusWithValue<Transform, TransformEvent>
    {
        
    }
}