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

    public void UpdateSlot(InventoryItem newItem)
    {
        if(newItem.Quantity > 0)
        {
            currentItem = newItem;
            iconImage.enabled = true;
            iconImage.sprite = currentItem.ItemData.ItemIcon;
            quantityText.text = currentItem.Quantity.ToString();
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
}
