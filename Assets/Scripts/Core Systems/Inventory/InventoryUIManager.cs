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
                inventorySlots[i].UpdateSlot(inventory[i]);
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
}
