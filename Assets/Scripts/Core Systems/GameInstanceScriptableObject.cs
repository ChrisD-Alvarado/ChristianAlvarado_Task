using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "GameInstanceSO", menuName = "Core/GameInstanceSO")]
public class GameInstanceScriptableObject : ScriptableObject
{
    static GameInstanceScriptableObject instance;
    public static GameInstanceScriptableObject Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Resources.Load<GameInstanceScriptableObject>("GlobalGameInstanceSO");
                //Try to load Player inventory
                //If load failed, Player Inventory should be populated with default inventory
                instance.PlayerInventory.PopulateInventory(instance.DefaultPlayerInventory);
            }

            if(instance == null)
            {
                Debug.LogError("Error Loading Global Game Instance");
            }

            return instance;
        }
    }

    [SerializeField]
    PlayerInventory defaultInventory;
    public PlayerInventory DefaultPlayerInventory { get { return defaultInventory; } }

    [SerializeField]
    List<ItemDataScriptableObject> allItems;

    public PlayerInventory PlayerInventory { private set; get; }

    public void LoadPlayerInventory(Dictionary<string, int> items)
    {
        foreach(string s in items.Keys)
        {
            if (ItemExists(s))
            {
                PlayerInventory.AddItemToInventory(GetItemByName(s), items[s]);
            }
        }
    }

    public ItemDataScriptableObject GetItemByName(string itemName)
    {
        foreach (ItemDataScriptableObject i in allItems)
        {
            if (i.ItemName == itemName)
            {
                return i;
            }
        }

        Debug.LogError($"Item {itemName} does not exist.");
        return null;
    }

    public bool ItemExists(string itemName)
    {
        bool result = false;

        foreach (ItemDataScriptableObject i in allItems)
        {
            if (i.ItemName == itemName)
            {
                result = true;
                break;
            }
        }

        return result;
    }

    public void SaveInventory()
    {

    }
}
