using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUISlot : MonoBehaviour
{
    InventoryItem currentItem = new InventoryItem();
    
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

    //TODO: Add Click interactions

    public void UpdateSlot(InventoryItem newItem)
    {
        if(newItem.Quantity > 0)
        {
            currentItem = newItem;
            iconImage.enabled = true;
            iconImage.sprite = currentItem.ItemData.ItemIcon;
            quantityText.text = $"x{currentItem.Quantity.ToString()}";
            tooltipText.text = currentItem.ItemData.Description;
            slotButton.interactable = currentItem.ItemData.Consumable;
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
            InventoryUIManager.Instance.SelectItem(currentItem);
        }
    }
}
