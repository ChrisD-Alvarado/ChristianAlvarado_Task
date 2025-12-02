using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConfirmInventoryWindow : MonoBehaviour
{
    [SerializeField]
    Image selectedItemIcon;

    [SerializeField]
    TextMeshProUGUI confirmTMP;

    public void ItemWasSelected(InventoryItem selectedItem)
    {
        selectedItemIcon.sprite = selectedItem.ItemData.ItemIcon;
        confirmTMP.text = $"Do you really want to consume {selectedItem.ItemData.ItemName}?";
    }
}
