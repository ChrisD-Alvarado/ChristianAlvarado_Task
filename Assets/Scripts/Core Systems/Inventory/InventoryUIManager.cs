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

    [SerializeField]
    float startDragDelay = 0.5f;
    public float StartDragDelay {  get { return startDragDelay; } }

    public bool IsDragging { private set; get; } = false;
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
                inventorySlots[i].SetSlot(i);
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

    public bool IsSlotAssigned(int slot)
    {
        return inventorySlots[slot].IsAssigned;
    }

    public void HideSwapIcons()
    {
        foreach(InventoryUISlot i in inventorySlots)
        {
            i.HideSwapIcon();
        }
    }

    InventoryItem tempItem;
    public void SwapSlots(int slotA, int slotB)
    {
        tempItem = inventorySlots[slotA].CurrentItem;
        inventorySlots[slotA].UpdateSlot(inventorySlots[slotB].CurrentItem, slotA);
        inventorySlots[slotB].UpdateSlot(tempItem, slotB);
    }

    public void SwapWithDraggedSlot(int slotToSwap)
    {
        if (inventorySlots[slotToSwap].IsAssigned)
        {
            SwapSlots(draggedSlot, slotToSwap);
        }
        else
        {
            inventorySlots[slotToSwap].UpdateSlot(dragSlot.DraggedItem, slotToSwap);
            inventorySlots[draggedSlot].ClearSlot();
        }
        inventorySlots[slotToSwap].HideSwapIcon();
    }

    public void StartDrag(int slot)
    {
        draggedSlot = slot;
        IsDragging = true;
        dragSlot.DragItem(inventorySlots[draggedSlot].CurrentItem, inventorySlots[draggedSlot]);
        inventorySlots[draggedSlot].ClearSlot();
    }

    public void StopDragging()
    {
        Debug.Log("Stop Dragging");
        if (IsDragging)
        {
            dragSlot.StopDraggingItem();
            inventorySlots[dragSlot.SlotUnderItem].UpdateSlot(dragSlot.DraggedItem, dragSlot.SlotUnderItem);
        }
        IsDragging = false;
    }
}