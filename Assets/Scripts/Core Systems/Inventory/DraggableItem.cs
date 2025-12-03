using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class DraggableItem : MonoBehaviour
{
    [SerializeField]
    Image itemIcon;

    InventoryItem draggedItem = new InventoryItem();
    public InventoryItem DraggedItem { get { return draggedItem; } }

    public int SlotUnderItem = -1;

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
        rectTransform.position = Mouse.current.position.ReadValue();
        itemIcon.sprite = item.ItemData.ItemIcon;
        SlotUnderItem = selectedSlot.CurrentSlot;
    }

    public void StopDraggingItem()
    {
        itemIcon.enabled = false;
        isBeingDragged = false;

        InventoryUIManager.Instance.SwapWithDraggedSlot(SlotUnderItem);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "UISlot")
        {
            SlotUnderItem = collision.GetComponent<InventoryUISlot>().CurrentSlot;
            Debug.Log($"Drag Item is over slot {SlotUnderItem}");
            InventoryUIManager.Instance.HideSwapIcons();
            collision.GetComponent<InventoryUISlot>().SetSwapIcon(draggedItem.ItemData.ItemIcon);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "UISlot")
        {
            Debug.Log($"Drag Item leaves slot {SlotUnderItem}");
            //collision.GetComponent<InventoryUISlot>().HideSwapIcon();
        }
    }
}
