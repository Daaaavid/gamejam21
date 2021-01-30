using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Interaction
{
    public class InteractableObject : MonoBehaviour
    {
        public InteractionBus InteractionBus;
        public Sprite InventoryIcon;

        public bool ConsumeOnUse;
        public float DestroyOnConsumeDelay = 0.5f;
        
        public bool IsInventoryItem;
        public bool IsInPlayerHand;
        private bool _interactable = true;
        
        public UnityEvent OnInteract;
        public readonly InteractableObjectEvent OnUse = new InteractableObjectEvent();
        public readonly InteractableObjectEvent OnStopUse = new InteractableObjectEvent();

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
            if (!_interactable) return;
            if (Input.GetMouseButtonUp(0))
            {
                //Left click
                if(!IsInPlayerHand)View();
                else
                {
                    OnUse.Invoke(this);
                }
            }
            if (Input.GetMouseButtonUp(1))
            {
               //right click
               if(!IsInPlayerHand)Interact();
               else
               {
                   OnStopUse.Invoke(this);
               }
            }
        }

        public void Consume(UnityAction<InteractableObject> OnConsumed)
        {
            _interactable = false;
            StartCoroutine(_Consume(OnConsumed));
        }

        public IEnumerator _Consume(UnityAction<InteractableObject> OnConsume)
        {
            yield return new WaitForSeconds(DestroyOnConsumeDelay);
            OnConsume.Invoke(this);
            Destroy(gameObject);
        }

    }
}