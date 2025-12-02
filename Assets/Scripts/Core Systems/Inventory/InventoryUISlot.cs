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

    //TODO: Add Tooltip
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

    public void ShowTooltip()
    {
        tooltip.SetActive(true);
    }

    public void HideTooltip()
    {
        tooltip.SetActive(false);
    }
}
