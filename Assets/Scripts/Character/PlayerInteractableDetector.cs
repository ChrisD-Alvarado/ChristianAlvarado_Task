using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerInteractableDetector : MonoBehaviour
{
    PlayerController playerController;
    Collider2D collider;

    Interactable currentInteractable;

    public bool DetectsInteractable { private set; get; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Interactable")
        {
            DetectsInteractable = true;
            currentInteractable = collision.GetComponent<Interactable>();
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
}
