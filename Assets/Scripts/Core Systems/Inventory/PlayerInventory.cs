using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Events;

[Serializable]
public class PlayerInventory
{
    [SerializeField]
    List<InventoryItem> inventoryItems = new List<InventoryItem>();

    public bool ItemSelected { private set; get; } = false;
    public InventoryItem CurrentSelectedItem { private set; get; } = new InventoryItem();

    //Updated Event
    public Action<List<InventoryItem>> InventoryUpdatedAction;

    public bool AddItemToInventory(ItemDataScriptableObject item, int quantity)
    {
        int firstEmptyslot = -1;
        bool foundItem = false;

        for (int i = inventoryItems.Count -1; i >= 0; i--)
        {
            if(inventoryItems[i].Quantity <= 0)
            {
                firstEmptyslot = i;
            }
            else if (inventoryItems[i].ItemData == item)
            {
                foundItem = true;
                SetIventorySlot(i, item, inventoryItems[i].Quantity + quantity);
                DebugInventory();
                return true;
            }
        }

        if (!foundItem && firstEmptyslot >= 0)
        {
            SetIventorySlot(firstEmptyslot, item, quantity);
            DebugInventory();
            return true;
        }

        Debug.LogWarning($"There are not empty inventory slots");
        return false;
    }

    public bool ConsumeSelectedItemFromInventory()
    {
        bool result = RemoveItemFromInventory(CurrentSelectedItem.ItemData);

        //Handle Consumed Item effects

        return result;
    }

    public bool RemoveItemFromInventory(ItemDataScriptableObject item, int quantity = 1)
    {
        for(int i = 0; i < inventoryItems.Count; i++)
        {
            if(inventoryItems[i].ItemData == item && inventoryItems[i].Quantity > 0)
            {
                SetIventorySlot(i, item, inventoryItems[i].Quantity - quantity);
                DebugInventory();
                return true;
            }
        }

        Debug.LogWarning($"Trying to remove {quantity} {item.ItemName} but player doesn't have the item");
        return false;
    }

    public void SetIventorySlot(int slot, ItemDataScriptableObject item, int quantity)
    {
        if(quantity < 0)
        {
            //Never have negative items in inventory
            quantity = 0;
        }

        inventoryItems[slot].SetItem(item, quantity);
        InventoryUpdatedAction?.Invoke(inventoryItems);
    }

    public int HowManyOfItem(ItemDataScriptableObject item)
    {
        int result = 0;
        foreach(InventoryItem s in inventoryItems)
        {
            if(s.Quantity > result && s.ItemData == item)
            {
                result = s.Quantity;
                break;
            }
        }

        return result;
    }

    public void PopulateInventory(PlayerInventory inventoryToCopy)
    {
        inventoryItems.Clear();
        for(int i = 0; i < inventoryToCopy.inventoryItems.Count; i++)
        {
            inventoryItems.Add(new InventoryItem());
            if(inventoryToCopy.inventoryItems[i].Quantity > 0)
            {
                inventoryItems[i].SetItem(inventoryToCopy.inventoryItems[i].ItemData, inventoryToCopy.inventoryItems[i].Quantity);
            }
        }

        InventoryUpdatedAction?.Invoke(inventoryItems);
        DebugInventory();
    }

    public void SetSelectedItem(InventoryItem selectedItem)
    {
        CurrentSelectedItem = selectedItem;
        ItemSelected = true;
    }
    public void DeselectItem()
    {
        ItemSelected = false;
    }

    void DebugInventory()
    {
        string log = "Inventory: ";
        for(int i= 0; i < inventoryItems.Count; i++)
        {
            log += $"Slot {i}: ";
            if(inventoryItems[i].Quantity > 0)
            {
                log += $"{inventoryItems[i].Quantity} {inventoryItems[i].ItemData.ItemName}s ";
            }
            else
            {
                log += "empty. ";
            }
        }

        Debug.Log(log);
    }
}
