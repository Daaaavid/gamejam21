using UnityEngine;
using UnityEngine.Events;

namespace Interaction
{
    public class InteractableObject : MonoBehaviour
    {
        public InteractionBus InteractionBus;
        
        public UnityEvent OnInteract;

        //public Narrative?;
        
        public void Interact()
        {
            InteractionBus.SetValue(this);
            OnInteract.Invoke();
        }

        public void View()
        {
            //Narrative.Start/play/stuff
        }
    }
}