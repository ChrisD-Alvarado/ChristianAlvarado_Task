using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerInteractableDetector : MonoBehaviour
{
    PlayerController playerController;

    Interactable currentInteractable;

    public bool DetectsInteractable { private set; get; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Interactable")
        {
            currentInteractable = collision.GetComponent<Interactable>();
            if(currentInteractable.State != InteractableState.Hidden)
            {
                DetectsInteractable = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Interactable")
        {
            DetectsInteractable = false;
            currentInteractable = null;
        }
    }

    public InteractableKind GetInteractableKind()
    {
        return currentInteractable.Kind;
    }

    public void SendInteractMessage()
    {
        if(DetectsInteractable && currentInteractable.State != InteractableState.Hidden)
        {
            currentInteractable.Interact();
            if (currentInteractable.HidesOnFirstInteraction)
            {
                DetectsInteractable = false;
                currentInteractable = null;
            }
        }
    }
}
