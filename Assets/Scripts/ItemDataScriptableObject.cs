using UnityEngine;

[CreateAssetMenu(fileName = "ItemSO", menuName = "Items/ItemData")]
public class ItemDataScriptableObject : ScriptableObject
{
    [SerializeField]
    Sprite itemIcon;
    public Sprite ItemIcon { get { return itemIcon; } }

    [SerializeField]
    Sprite itemSprite;
    public Sprite ItemSprite { get { return itemSprite; } }

    [SerializeField]
    string itemName;
    public string ItemName { get { return ItemName; } }

    [SerializeField]
    string description;
    public string Description { get { return description; } }

    [SerializeField]
    bool equipment;
    public bool Equipment { get { return equipment; } }
}
