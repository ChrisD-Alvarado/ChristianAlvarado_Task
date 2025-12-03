using UnityEngine;

public class ChoppableTreeInteractable : Interactable
{
    [SerializeField]
    int hitsRequired = 3;

    [SerializeField]
    int maxHits = 13;

    int hitsTaken = 0;

    [SerializeField]
    PickUpItemInteractable spawnedItem;

    [SerializeField]
    ItemDataScriptableObject itemToSpawn;

    [SerializeField]
    int spawnedAmount = 1;

    private void Awake()
    {
        kind = InteractableKind.ChoppableTree;
    }

    public override void Interact()
    {
        hitsTaken++;

        if (hitsTaken % hitsRequired == 0)
        {
            spawnedItem.SpawnOrAdd(itemToSpawn, spawnedAmount);
        }

        if (hitsTaken >= maxHits)
        {
            state = InteractableState.Locked;
        }
    }
}
