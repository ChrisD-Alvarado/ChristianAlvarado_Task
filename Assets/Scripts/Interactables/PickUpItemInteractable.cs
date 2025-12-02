using UnityEngine;

public class PickUpItemInteractable : Interactable
{
    [SerializeField]
    ItemDataScriptableObject itemData;

    [SerializeField]
    int quantity;

    private void Awake()
    {
        kind = InteractableKind.PickableItem;
    }

    public override void Interact()
    {

    }
}
