using System;
using System.Collections;
using Subtegral.DialogueSystem.DataContainers;
using UnityEngine;
using UnityEngine.Events;

namespace Interaction
{
    public class InteractableObject : MonoBehaviour
    {
        public InteractionBus InteractionBus;
        public bool ConsumeOnUse;
        public float DestroyOnConsumeDelay = 0.5f;
        
        [Header("Inventory")]
        public Sprite InventoryIcon;
        public string TextOnHover;
        private bool _interactable = true;
        public bool IsInventoryItem;
        [HideInInspector]public bool IsInPlayerHand;
        
        [Space(10)]
        [Header("Audio events")]
        public UnityEvent OnInteractAudio;
        public UnityEvent OnUseAudio;
        public UnityEvent OnStopUseAudio;
        [Tooltip("Pas de 'DestroyOnConsumeDelay' aan naar minimaal de audiolengte!")] public UnityEvent OnConsumeAudio;
        
        public readonly InteractableObjectEvent OnUse = new InteractableObjectEvent();
        public readonly InteractableObjectEvent OnStopUse = new InteractableObjectEvent();
       
        [Space(10)]
        [Header("Dialogue")]
        public InteractionBus DialogBus;
        public DialogueContainer Dialogue;
        public int SplitValue;
        
        [Space(10)]
        [Header("Movement")]
        public InteractionBus MoveSystem;
        public TransformBus MoveSystem2;
        private bool proximity;
        public bool ReturnOnDone;
        public Transform TargetPositionOnDone;

        public bool BypassMovement;
        
        public void Interact()
        {
            MoveSystem.SetValue(this);

            if (BypassMovement)
            {
                InteractionBus.SetValue(this);
                OnInteractAudio.Invoke();
            }
            StartCoroutine(WaitForMovement(
                () =>
                {
                    InteractionBus.SetValue(this);
                    OnInteractAudio.Invoke();
                    if(ReturnOnDone) MoveSystem.OnInvokeReturnPlayer.Invoke();
                    else
                    {
                        MoveSystem2.SetValue(TargetPositionOnDone);
                    }
                }));
        }

        private bool Proximity()
        {
            return proximity;
        }

        IEnumerator WaitForMovement(UnityAction OnDone)
        {
            MoveSystem.OnProximity.AddListener(() => proximity = true);
            yield return new WaitUntil(Proximity);
            proximity = false;
            MoveSystem.OnProximity.RemoveListener(() => proximity = true);
            OnDone.Invoke();
        }

        public void View()
        {
            DialogBus.SetValue(this);
        }

        private void OnMouseOver()
        {
            if (!_interactable) return;
            if (Input.GetMouseButtonUp(0))
            {
                Debug.Log("leftClick");
                //Left click
                if(!IsInPlayerHand)View();
                else
                {
                    OnUseAudio.Invoke();
                    OnUse.Invoke(this);
                }
            }
            if (Input.GetMouseButtonUp(1))
            {
                Debug.Log("rightClick");
               //right click
               if(!IsInPlayerHand)Interact();
               else
               {
                   OnStopUseAudio.Invoke();
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
            OnConsumeAudio.Invoke();
            yield return new WaitForSeconds(DestroyOnConsumeDelay);
            OnConsume.Invoke(this);
            Destroy(gameObject);
        }

    }
}