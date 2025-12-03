using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUISlot : MonoBehaviour
{
    public InventoryItem CurrentItem { private set; get; } = new InventoryItem();
    
    [SerializeField]
    Image iconImage;

    [SerializeField]
    Image swapIcon;

    [SerializeField]
    TextMeshProUGUI quantityText;

    [SerializeField]
    Button slotButton;

    [SerializeField]
    GameObject tooltip;

    [SerializeField]
    TextMeshProUGUI tooltipText;
    
    float dragStartTime;
    bool isClicked = false;

    public bool IsAssigned { private set; get; } = false;

    public int CurrentSlot { private set; get; }

    public void UpdateSlot(InventoryItem newItem, int slot)
    {
        CurrentSlot = slot;

        if(newItem.Quantity > 0)
        {
            CurrentItem = newItem;
            iconImage.enabled = true;
            iconImage.sprite = CurrentItem.ItemData.ItemIcon;
            quantityText.text = $"x{CurrentItem.Quantity.ToString()}";
            tooltipText.text = CurrentItem.ItemData.Description;
            slotButton.interactable = CurrentItem.ItemData.Consumable;
            IsAssigned = true;
        }
        else
        {
            SetSlot(slot);
            ClearSlot();
        }
    }

    public void ClearSlot()
    {
        iconImage.enabled = false;
        quantityText.text = "";
        IsAssigned = false;
        HideSwapIcon();
    }

    public void SetSlot(int slot)
    {
        CurrentSlot = slot;
    }

    void ShowTooltip(bool show)
    {
        tooltip.SetActive(show);
    }

    public void SetSlotSelected(bool selected)
    {
        ShowTooltip(selected);
    }

    public void SelectItem(bool selected)
    {
        if (selected)
        {
            InventoryUIManager.Instance.SelectItem(CurrentItem);
        }
    }

    public void OnSlotClicked()
    {
        Debug.Log($"Slot {CurrentSlot} clicked");
        dragStartTime = Time.time;
        isClicked = true;
    }

    public void OnStoppedClick()
    {
        Debug.Log($"Slot {CurrentSlot} stopped click");
        if (isClicked && Time.time - dragStartTime < InventoryUIManager.Instance.StartDragDelay && CurrentItem.ItemData.Consumable)
        {
            InventoryUIManager.Instance.SelectItem(CurrentItem);
        }

        isClicked = false;
        if (InventoryUIManager.Instance.IsDragging)
        {
            InventoryUIManager.Instance.StopDragging();
        }
    }

    private void Update()
    {
        if (isClicked)
        {
            if (Time.time - dragStartTime >= InventoryUIManager.Instance.StartDragDelay)
            {
                InventoryUIManager.Instance.StartDrag(CurrentSlot);
                isClicked = false;
            }
        }
    }

    public void SetSwapIcon(Sprite icon)
    {
        swapIcon.sprite = icon;
        swapIcon.enabled = true;
    }

    public void HideSwapIcon()
    {
        swapIcon.enabled = false;
    }
}
