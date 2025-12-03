using UnityEngine;
using UnityEngine.UI;

public class DraggableItemSlot : MonoBehaviour
{
    [SerializeField]
    Image itemIcon;

    InventoryItem draggedItem;

    InventoryUISlot slotUnderItem;

    bool isBeingDragged = false;

    private void Awake()
    {
        StopDraggingItem();
    }

    private void Update()
    {
        transform.position = Input.mousePosition;
    }

    public void DragItem(InventoryItem item, InventoryUISlot selectedSlot)
    {
        draggedItem = item;
        itemIcon.enabled = true;
        isBeingDragged = true;
        itemIcon.sprite = item.ItemData.ItemIcon;
        slotUnderItem = selectedSlot;
        slotUnderItem.ClearSlot();
    }

    public void StopDraggingItem()
    {
        itemIcon.enabled = false;
        isBeingDragged = false;

        slotUnderItem.UpdateSlot(draggedItem, slotUnderItem.CurrentSlot);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "UISlot")
        {
            slotUnderItem = collision.GetComponent<InventoryUISlot>();
            Debug.Log($"Drag Item is over slot {slotUnderItem.CurrentSlot}");

            //Move item left or right
            if(transform.position.x > collision.transform.position.x)
            {
                InventoryUIManager.Instance.MoveItemRight(slotUnderItem.CurrentSlot);
            }
            else
            {
                InventoryUIManager.Instance.MoveItemLeft(slotUnderItem.CurrentSlot);
            }
        }
    }
}
