using UnityEngine;

[CreateAssetMenu(fileName = "InventorySlotSO", menuName = "Inventory/InventorySlot")]
public class InventorySlotScriptableObject : ScriptableObject
{
    public ItemDataScriptableObject currentItem;
    public int quantity;
}
