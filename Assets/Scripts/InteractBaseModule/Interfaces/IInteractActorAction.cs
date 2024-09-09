using UnityEngine;

public interface IInteractActorAction
{
    public bool IsActionForInteractable(IInteractable interact);

    public bool Act(InteractActor actor, GameObject interact);
}
