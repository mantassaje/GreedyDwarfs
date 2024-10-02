using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Actors that can interact with IInteractable by using IInteractActorAction implementations.
/// </summary>
public class InteractActor : MonoBehaviour
{
    private class Interactable
    {
        public IInteractable Interact { get; set; }
        public GameObject GameObject { get; set; }
    }

    public IInteractActorAction[] InteractActorActions { get; private set; }
    public PhotonView PhotonView { get; private set; }
    public PlayerToken Player { get; private set; }

    private void Awake()
    {
        InteractActorActions = GetComponents<IInteractActorAction>();
        PhotonView = GetComponent<PhotonView>();
        Player = GetComponent<PlayerToken>();
    }

    private Interactable GetInteractableInMousePosition()
    {
        Vector3 mouse = Input.mousePosition;
        var worldPosition = Camera.main.ScreenToWorldPoint(mouse);
        var hit = Physics2D.Raycast(worldPosition, -Vector2.up);

        if (hit.collider != null)
        {
            var interact = hit.collider.GetComponent<IInteractable>();
            return new Interactable()
            {
                GameObject = hit.collider.gameObject,
                Interact = interact
            };
        }

        return null;
    }

    public IInteractActorAction GetAction(IInteractable interact)
    {
        foreach(var action in InteractActorActions)
        {
            if (action.IsActionForInteractable(interact))
            {
                return action;
            }
        }

        return null;
    }

    public bool InteractWithObjectInMousePosition()
    {
        var interact = GetInteractableInMousePosition();

        if (interact != null)
        {
            var action = GetAction(interact.Interact);

            if (action != null)
            {
                action.Act(this, interact.GameObject);
            }
        }

        return false;
    }
}
