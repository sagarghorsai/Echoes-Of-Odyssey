using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    [SerializeField] private GameObject container;
    private List<Interactable> interactables = new List<Interactable>();

    public void Show()
    {
        container.SetActive(true);
    }

    public void Hide()
    {
        container.SetActive(false);
    }


    public void RegisterInteractable(Interactable interactable)
    {
        if (!interactables.Contains(interactable))
        {
            interactables.Add(interactable);
        }
    }

    public void UnregisterInteractable(Interactable interactable)
    {
        if (interactables.Contains(interactable))
        {
            interactables.Remove(interactable);
        }
    }

}