using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "InventorySO", menuName = "Inventory/Inventory")]
public class PlayerInventoryScriptableObject : ScriptableObject
{
    [SerializeField]
    List<InventorySlotScriptableObject> inventoryItems;

    InventorySlotScriptableObject selectedItem;

    //Updated Event

    public bool AddItemToInventory(ItemDataScriptableObject item, int quantity)
    {
        int firstEmptyslot = -1;
        bool foundItem = false;

        for (int i = inventoryItems.Count -1; i >= 0; i--)
        {
            if(inventoryItems[i].quantity <= 0)
            {
                firstEmptyslot = i;
            }
            else if (inventoryItems[i].currentItem == item)
            {
                foundItem = true;
                SetIventorySlot(i, item, inventoryItems[i].quantity + quantity);
                break;
            }
        }

        if (!foundItem && firstEmptyslot >= 0)
        {
            SetIventorySlot(firstEmptyslot, item, quantity);
            return true;
        }

        Debug.LogWarning($"There are not empty inventory slots");
        return false;
    }

    public bool RemoveItemFromInventory(ItemDataScriptableObject item, int quantity)
    {
        for(int i = 0; i < inventoryItems.Count; i++)
        {
            if(inventoryItems[i].currentItem == item && inventoryItems[i].quantity > 0)
            {
                SetIventorySlot(i, item, inventoryItems[i].quantity - quantity);
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

        inventoryItems[slot].quantity = quantity;
        inventoryItems[slot].currentItem = item;
    }

    public int HowManyOfItem(ItemDataScriptableObject item)
    {
        int result = 0;
        foreach(InventorySlotScriptableObject s in inventoryItems)
        {
            if(s.currentItem == item && s.quantity > result)
            {
                result = s.quantity;
            }
        }

        return result;
    }
}
