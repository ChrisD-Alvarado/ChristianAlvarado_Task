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
                instance.LoadPlayerInventory();
            }

            if(instance == null)
            {
                Debug.LogError("Error Loading Global Game Instance");
            }

            return instance;
        }
    }

    public static bool AlreadyLoaded { private set; get; } = false;

    [SerializeField]
    PlayerInventory defaultInventory;
    public PlayerInventory DefaultPlayerInventory { get { return defaultInventory; } }

    [SerializeField]
    List<ItemDataScriptableObject> allItems;

    public ItemDataScriptableObject DefaultItem { get { return allItems[0]; } }

    public PlayerInventory PlayerInventory { private set; get; } = new PlayerInventory();

    public void LoadPlayerInventory()
    {
        //Load Player Inventory
        Dictionary<string, int> playerInventoryDictionary = new Dictionary<string, int>();
        //playerInventoryDictionary = Load Inventory

        if(playerInventoryDictionary.Keys.Count < 1)
        {
            instance.PlayerInventory.PopulateInventory(instance.DefaultPlayerInventory);
        }

        AlreadyLoaded = true;
    }

    public void PopulatePlayerInventory(Dictionary<string, int> items)
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
