using System;
using Interaction;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Inventory
{
    public class InventorySlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public Tooltip tooltip;
        
        public Sprite EmptyImage;

        private bool _interactable => !Gray.gameObject.activeSelf;
        public Sprite ItemIcon => _item.InventoryIcon;
        public Image Target;
        public Image Gray;
        public bool HasItem => _item != null;
        
        private InteractableObject _item;

        private Inventory _inventory;

        private void Awake()
        {
            _inventory = transform.parent.GetComponentInParent<Inventory>();
        }

        public void SetItem(InteractableObject obj)
        {
            _item = obj;
            Target.sprite = HasItem ? ItemIcon : EmptyImage;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!_item || !_interactable) return;
            _inventory.TakeInHand(_item);
            Gray.gameObject.SetActive(true);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            //tooltip show
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            //tooltip hide
        }
    }
}