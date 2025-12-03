using UnityEngine;

public class NPCInteractable : Interactable
{

    private void Awake()
    {
        kind = InteractableKind.NPC;
    }

    public override void Interact()
    {
        //Trigger Dialogue
        Debug.Log($"Speaking with: {Name}");
    }
}
