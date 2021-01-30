using System;
using UnityEngine;
using UnityEngine.Events;

namespace Interaction
{
    public class InteractableObject : MonoBehaviour
    {
        public InteractionBus InteractionBus;
        public Sprite InventoryIcon;

        public bool ConsumeOnUse;
        public bool IsInventoryItem;
        
        public UnityEvent OnInteract;

        public string TextOnHover;
        
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

        private void OnMouseOver()
        {
            if (Input.GetMouseButtonUp(0))
            {
                //Left click
                View();
            }
            if (Input.GetMouseButtonUp(1))
            {
               //right click
               Interact();
            }
        }
    }
}