/*
 * Code by: Chris D. Alvarado
 * Created on: December 2025
 * Last Update: December 1st 2025
 * 
 * */
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class Interactable : MonoBehaviour
{
    [SerializeField]
    string name;
    public string Name
    {
        get
        {
            return name;
        }
    }

    [SerializeField]
    string interactLabel = "Interact";
    public string Label
    {
        get
        {
            return interactLabel;
        }
    }

    [SerializeField]
    protected InteractableState state = InteractableState.Detectable;
    public InteractableState State
    {
        get
        {
            return state;
        }
    }

    [SerializeField]
    protected InteractableKind kind;
    public InteractableKind Kind
    {
        get
        {
            return kind;
        }
    }

    [SerializeField]
    protected bool hidesOnFirstInteraction = true;
    public bool HidesOnFirstInteraction
    {
        get
        {
            return hidesOnFirstInteraction;
        }
    }


    public virtual void Interact() { }
}


public enum InteractableState
{
    Detectable,
    Locked,
    Hidden
}

public enum InteractableKind
{
    NPC,
    Chest,
    Door,
    PickableItem,
    ChoppableTree
}