using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class DraggableItem : MonoBehaviour
{
    [SerializeField]
    Image itemIcon;

    InventoryItem draggedItem = new InventoryItem();

    InventoryUISlot slotUnderItem = new InventoryUISlot();

    bool isBeingDragged = false;

    RectTransform rectTransform;

    private void Awake()
    {
        itemIcon.enabled = false;
        isBeingDragged = false;
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (isBeingDragged)
        {
            rectTransform.position = Mouse.current.position.ReadValue();
        }
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
