using System;
using System.Collections.Generic;
using Interaction;
using UnityEngine;

namespace Inventory
{
    public class Inventory : MonoBehaviour
    {
        private List<InteractableObject> itemList = new List<InteractableObject>();

        public Transform playerHandPosition;
        
        public InteractionBus Bus;

        private void Awake()
        {
            Bus.OnChange.AddListener(CheckState);
        }

        public void CheckState(InteractableObject obj)
        {
            if (!obj.IsInventoryItem) return;
            if (!itemList.Contains(obj))
            {
                AddToInventory(obj);
            }
        }

        public void AddToInventory(InteractableObject obj)
        {
            obj.gameObject.SetActive(false); //todo add an animation for this?
            itemList.Add(obj);
            GetFreeInventorySlot().SetItem(obj);
        }

        private InventorySlot GetFreeInventorySlot()
        {
            var slots = GetComponentsInChildren<InventorySlot>();
            foreach (var inventorySlot in slots)
            {
                if (!inventorySlot.HasItem) return inventorySlot;
            }
            Debug.Log("no free slots");
            return null;
        }

        public void TakeInHand(InteractableObject obj)
        {
            obj.gameObject.SetActive(true);
            obj.transform.position = playerHandPosition.position;
        }
    }
}