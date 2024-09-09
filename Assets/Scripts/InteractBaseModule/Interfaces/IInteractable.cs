using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Object that InteractActor can interact with.
/// </summary>
public interface IInteractable
{
    /// <summary>
    /// Make actors interact with this object.
    /// This method manipulates only IInteractable data.
    /// Actor data is modified by a calling method.
    /// </summary>
    public bool Interact(InteractActor actor);
}

