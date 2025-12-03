using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;

public class SaveManager : MonoBehaviour
{
    static SaveManager instance;
    public static SaveManager Instance { 
        get
        {
            if (instance == null)
            {
                GameObject newInstance = new GameObject();
                newInstance.name = "SaveManager";
                instance = newInstance.AddComponent<SaveManager>();
            }

            return instance;
        }
    }

    string filename = "Inventory.CATask";

    public void SaveInventory(List<InventoryItem> inventoryList)
    {
        string inventoryString = "";
        foreach(InventoryItem i in inventoryList)
        {
            inventoryString += $"{inventoryList.IndexOf(i)}-{i.ItemData.ItemName}-{i.Quantity},";
        }
        Debug.Log(inventoryString);

        try
        {
            string path = Application.persistentDataPath + "/" + filename;
            using (StreamWriter writer = new StreamWriter(path, false, System.Text.Encoding.Unicode))
            {
                Debug.LogFormat("Path: {0}", path);
                writer.WriteLine(inventoryString);
                writer.Flush();
                writer.Close();

            }
        }
        catch(System.Exception e)
        {
            Debug.LogErrorFormat("Couldn't save string {0}. {1}", filename, e.Message);
        }
    }

    public PlayerInventory LoadInventory()
    {
        PlayerInventory result = new PlayerInventory();
        result.PopulateInventory(GameInstanceScriptableObject.Instance.DefaultPlayerInventory);
        string path = Application.persistentDataPath + "/" + filename;
        string inventoryString = "";

        if (FileExists(path))
        {
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    inventoryString = sr.ReadToEnd();
                }
            }
            catch (System.Exception e)
            {
                Debug.LogErrorFormat("Couldn't load string {0}. {1}", filename, e.Message);
            }
        }
        else
        {
            Debug.LogWarning("File does not exist");
        }

        List<InventoryItem> loadedInventory = new List<InventoryItem>();
        if(inventoryString.Length > 0)
        {
            List<string> itemsString = new List<string>();
            itemsString.AddRange(inventoryString.Split(","));

            foreach(string s in itemsString)
            {

                List<string> itemData = new List<string>();
                itemData.AddRange(s.Split("-"));

                if (itemData.Count < 3)
                {
                    break;
                }

                result.SetIventorySlot(Int32.Parse(itemData[0]), 
                    GameInstanceScriptableObject.Instance.GetItemByName(itemData[1]), Int32.Parse(itemData[2]));
            }
        }

        return result;
    }

    bool FileExists(string path)
    {
        FileInfo info = new FileInfo(path);
        return info.Exists;
    }
}
