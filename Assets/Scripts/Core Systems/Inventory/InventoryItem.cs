using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    [SerializeField]
    ItemDataScriptableObject itemData;
    public ItemDataScriptableObject ItemData { get { return itemData; } }

    [SerializeField]
    int quantity;

    public int Quantity { get { return quantity; } }

    public void SetItem(ItemDataScriptableObject newItem, int newQuantity)
    {
        itemData = newItem;
        quantity = newQuantity;
    }
}
