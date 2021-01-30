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
            if(itemList.Contains(obj)) GetInventorySlot(obj).SetItem(obj);
            else
            {
                itemList.Add(obj);
                GetFreeInventorySlot().SetItem(obj);
            }
            obj.gameObject.SetActive(false);
            obj.IsInPlayerHand = false;
            //todo add an animation for this?
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

        private InventorySlot GetInventorySlot(InteractableObject obj)
        {
            var slots = GetComponentsInChildren<InventorySlot>();
            foreach (var inventorySlot in slots)
            {
                if (inventorySlot.GetItem() == obj) return inventorySlot;
            }
            return null;
        }

        public void TakeInHand(InteractableObject obj)
        {
            obj.gameObject.SetActive(true);
            obj.IsInPlayerHand = true;
            obj.transform.position = playerHandPosition.position;
            obj.OnUse.AddListener(UseObject);
            obj.OnStopUse.AddListener(AddToInventory);
        }

        public void RemoveFromInventory(InteractableObject obj)
        {
            GetInventorySlot(obj)?.SetItem(null);
            itemList.Remove(obj);
        }

        public void UseObject(InteractableObject obj)
        {
            if (obj.ConsumeOnUse)
            {
                obj.Consume(RemoveFromInventory);
            }

        }
    }
}