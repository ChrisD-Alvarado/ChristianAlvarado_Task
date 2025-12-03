using UnityEngine;
using System.Collections.Generic;

public class InventoryUIManager : MonoBehaviour
{
    static InventoryUIManager instance;
    public static InventoryUIManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = GameObject.FindAnyObjectByType<InventoryUIManager>();
            }

            return instance;
        }
    }

    [SerializeField]
    List<InventoryUISlot> inventorySlots = new List<InventoryUISlot>();

    [SerializeField]
    ConfirmInventoryWindow confirmWindow;

    [SerializeField]
    DraggableItem dragSlot;

    bool isDragging = false;
    int draggedSlot = 0;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            inventorySlots.Clear();
            inventorySlots.AddRange(GetComponentsInChildren<InventoryUISlot>());
            GameInstanceScriptableObject.Instance.PlayerInventory.InventoryUpdatedAction += OnInventoryUpdated;
            GameInstanceScriptableObject.Instance.LoadPlayerInventory();
            ShowConfirmWindow(false);
        }
        else
        {
            Destroy(this);
        }
    }

    void OnInventoryUpdated(List<InventoryItem> inventory)
    {
        for(int i = 0; i < inventorySlots.Count; i++)
        {
            if(i < inventory.Count)
            {
                inventorySlots[i].UpdateSlot(inventory[i], i);
            }
            else
            {
                inventorySlots[i].ClearSlot();
            }
        }
    }

    public void SelectItem(InventoryItem selectedItem)
    {
        ShowConfirmWindow(true);
        confirmWindow.ItemWasSelected(selectedItem);
        GameInstanceScriptableObject.Instance.PlayerInventory.SetSelectedItem(selectedItem);
    }

    public void ShowConfirmWindow(bool show)
    {
        confirmWindow.gameObject.SetActive(show);

        if (!show)
        {
            GameInstanceScriptableObject.Instance.PlayerInventory.DeselectItem();
        }
    }

    public void ConfirmConsumeItem()
    {
        if (GameInstanceScriptableObject.Instance.PlayerInventory.ConsumeSelectedItemFromInventory())
        {
            Debug.Log("Item Consumed");
            //Handle consumed item UI consequences if needed
        }
        ShowConfirmWindow(false);
    }

    public void MoveItemRight(int slot)
    {
        if (slot + 1 >= inventorySlots.Count)
        {
            Debug.LogError("This is the last slot");
            return;
        }

        if (inventorySlots[slot].IsAssigned)
        {
            inventorySlots[slot + 1].UpdateSlot(inventorySlots[slot].CurrentItem, slot + 1);
            inventorySlots[slot].ClearSlot();
        }
        else { 
            Debug.Log("This slot is empty");
        }
    }

    public void MoveItemLeft(int slot)
    {
        if (slot - 1 < 0)
        {
            Debug.LogError("This is the first slot");
            return;
        }

        if (inventorySlots[slot].IsAssigned)
        {
            inventorySlots[slot - 1].UpdateSlot(inventorySlots[slot].CurrentItem, slot - 1);
            inventorySlots[slot].ClearSlot();
        }
        else { 
            Debug.Log("This slot is empty");
        }
    }

    public void StartDrag(int slot)
    {
        draggedSlot = slot;
        dragSlot.DragItem(inventorySlots[draggedSlot].CurrentItem, inventorySlots[draggedSlot]);
    }

    public void StopDragging()
    {
        if (isDragging)
        {
            dragSlot.StopDraggingItem();
        }
        isDragging = false;
    }
}