using System;
using UnityEngine;

namespace Interaction
{
    public class IObjColliderListener : MonoBehaviour
    {
        private InteractableObject _obj;

        private void Awake()
        {
            _obj = GetComponentInParent<InteractableObject>();
            if(_obj == null) Debug.LogWarning(name + " has no parent InteractableObject");
        }

        public void OnMouseOver()
        {
            _obj.OnMouseOverChild();
        }
    }
}