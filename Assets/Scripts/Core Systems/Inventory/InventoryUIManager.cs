using UnityEngine;
using System.Collections.Generic;

public class InventoryUIManager : MonoBehaviour
{
    static InventoryUIManager instance;
    public static InventoryUIManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = GameObject.FindAnyObjectByType<InventoryUIManager>();
            }

            return instance;
        }
    }

    [SerializeField]
    List<InventoryUISlot> inventorySlots = new List<InventoryUISlot>();


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            inventorySlots.Clear();
            inventorySlots.AddRange(GetComponentsInChildren<InventoryUISlot>());
            GameInstanceScriptableObject.Instance.PlayerInventory.InventoryUpdatedAction += OnInventoryUpdated;
            GameInstanceScriptableObject.Instance.LoadPlayerInventory();
        }
        else
        {
            Destroy(this);
        }
    }

    void OnInventoryUpdated(List<InventoryItem> inventory)
    {
        for(int i = 0; i < inventorySlots.Count; i++)
        {
            if(i < inventory.Count)
            {
                inventorySlots[i].UpdateSlot(inventory[i]);
            }
            else
            {
                inventorySlots[i].ClearSlot();
            }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
