using UnityEngine;

public class PickUpItemInteractable : Interactable
{
    [SerializeField]
    ItemDataScriptableObject itemData;

    [SerializeField]
    SpriteRenderer spriteRenderer;

    [SerializeField]
    bool spawnInAwake = false;

    bool spawnedInMap;

    [SerializeField]
    int quantity;

    private void Awake()
    {
        kind = InteractableKind.PickableItem;
        if (spawnInAwake)
        {
            SpawnOrAdd(itemData, quantity);
        }
    }

    public override void Interact()
    {
        //Make sure item is actually detectable
        if (state != InteractableState.Detectable)
        {
            return;
        }

        PlayerController player = FindAnyObjectByType<PlayerController>();
        player.GrantItem(itemData, quantity);

        if (hidesOnFirstInteraction)
        {
            HideItem();
        }
    }

    public void SpawnOrAdd(ItemDataScriptableObject item, int amount)
    {
        if (!spawnedInMap)
        {
            spawnedInMap = true;
            spriteRenderer.enabled = true;
            quantity = amount;
        }
        else
        {
            quantity += amount;
        }
    }

    void HideItem()
    {
        spriteRenderer.enabled = false;
        state = InteractableState.Hidden;
    }
}
