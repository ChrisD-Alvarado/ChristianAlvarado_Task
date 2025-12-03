using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUISlot : MonoBehaviour
{
    public InventoryItem CurrentItem { private set; get; } = new InventoryItem();
    
    [SerializeField]
    Image iconImage;

    [SerializeField]
    TextMeshProUGUI quantityText;

    [SerializeField]
    Button slotButton;

    [SerializeField]
    GameObject tooltip;

    [SerializeField]
    TextMeshProUGUI tooltipText;
    
    [SerializeField]
    float startDragDelay = 0.5f;
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
            ClearSlot();
        }
    }

    public void ClearSlot()
    {
        iconImage.enabled = false;
        quantityText.text = "";
        IsAssigned = false;
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
        if (isClicked && Time.time - dragStartTime < startDragDelay && CurrentItem.ItemData.Consumable)
        {
            InventoryUIManager.Instance.SelectItem(CurrentItem);
        }

        isClicked = false;
    }

    private void Update()
    {
        if (isClicked)
        {
            if (Time.time - dragStartTime >= startDragDelay)
            {
                InventoryUIManager.Instance.StartDrag(CurrentSlot);
                isClicked = false;
            }
        }
    }
}
