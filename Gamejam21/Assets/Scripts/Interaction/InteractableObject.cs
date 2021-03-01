using System;
using System.Collections;
using System.Collections.Generic;
using Subtegral.DialogueSystem.DataContainers;
using UnityEngine;
using UnityEngine.Events;

namespace Interaction
{
    public class InteractableObject : MonoBehaviour {
        private TwoDimensionalMovement player;

        public InteractionBus InteractionBus;
        public bool ConsumeOnUse;
        public float DestroyOnConsumeDelay = 0.5f;

        [Header("Inventory")]
        public Sprite InventoryIcon;
        public string TextOnHover;
        private bool _interactable = true;
        public bool IsInventoryItem;
        [HideInInspector] public bool IsInPlayerHand;

        [Space(10)]

        [Tooltip("Use in ChangeGlobalVariableUsingIndex the index of the int that you define here to change the global variable to this value.")]
        public List<GlobalInt> variableChanges = new List<GlobalInt>();

        [Header("Audio events")]
        public UnityEvent OnInteractAudio;
        public float InteractionDelay = 0;
        [Tooltip("Pas de 'InteractionDelay' aan naar minimaal de audiolengte van de interaction!")]
        public UnityEvent OnUseAudio;
        public UnityEvent OnStopUseAudio;
        public UnityEvent OnView;
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
        public float ProximityTreshold;
        public bool ReturnOnDone;
        public Transform TargetPositionOnDone;

        public bool BypassMovement;


        private void Start() {
            player = GameObject.FindWithTag("Player").GetComponent<TwoDimensionalMovement>();
        }

        public void Interact() {
            Debug.Log("InvokedInteract");
            MoveSystem.SetValue(this);

            if (player.State == 2) {
                InteractionBus.SetValue(this);
                OnInteractAudio.Invoke();
            }
            StartCoroutine(WaitForMovement(
                () => {
                    Debug.Log("InvokedDone");
                    InteractionBus.SetValue(this);
                    if (ReturnOnDone) MoveSystem.OnInvokeReturnPlayer.Invoke();
                    else {
                        MoveSystem2.SetValue(TargetPositionOnDone);
                    }
                }));
        }

        private bool Proximity() {
            return proximity;
        }

        IEnumerator WaitForMovement(UnityAction OnDone) {
            MoveSystem.OnProximity.AddListener(() => proximity = true);
            yield return new WaitUntil(Proximity);
            proximity = false;
            OnInteractAudio.Invoke();
            yield return new WaitForSeconds(InteractionDelay);
            MoveSystem.OnProximity.RemoveListener(() => proximity = true);
            OnDone.Invoke();
        }

        public void View() {
            if (player.State == 0 || player.State == 2) {
                DialogBus.SetValue(this);
                OnView.Invoke();
            }
        }
        
        public void OnMouseOverChild()
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

        private IEnumerator _Consume(UnityAction<InteractableObject> OnConsume)
        {
            OnConsumeAudio.Invoke();
            yield return new WaitForSeconds(DestroyOnConsumeDelay);
            OnConsume.Invoke(this);
            Destroy(gameObject);
        }

        public void ChangeGlobalVariableUsingIndex(int value) {
            ChangeGlobalVariable(variableChanges[value]);
        }
        void ChangeGlobalVariable(GlobalInt variable) {
            PlayerPrefs.SetInt(variable.key, variable.value);
        }

    }
}