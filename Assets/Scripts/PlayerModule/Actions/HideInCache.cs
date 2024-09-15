using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class HideInCache : MonoBehaviour, IInteractActorAction
{
    public Inventory Inventory { get; private set; }
    public InteractActor InteractActor { get; private set; }
    public PhotonView PhotonView { get; private set; }

    void Awake()
    {
        Inventory = GetComponent<Inventory>();
        InteractActor = GetComponent<InteractActor>();
        PhotonView = GetComponent<PhotonView>();
    }

    public bool Act(InteractActor actor, GameObject interact)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            return MasterHideInCache(interact);
        }
        else
        {
            if (!IsValidAction(interact))
            {
                return false;
            }

            PhotonView.RPC(nameof(RpcDoHideInCache), RpcTarget.MasterClient, GuidReferenceHelper.GetId(interact).ToString());

            return true;
        }
    }

    private bool IsValidAction(GameObject interact)
    {
        if (!Inventory.HasGold)
        {
            return false;
        }

        var distance = Vector2.Distance(InteractActor.transform.position, interact.transform.position);

        if (distance > 2f)
        {
            return false;
        }

        return true;
    }

    private bool MasterHideInCache(GameObject interact)
    {
        if (!IsValidAction(interact))
        {
            return false;
        }

        if (interact.GetComponent<IInteractable>().Interact(InteractActor))
        {
            Inventory.RemoveGold();
            return true;
        }

        return false;
    }

    [PunRPC]
    private void RpcDoHideInCache(string interactGuid)
    {
        var interact = GuidReferenceHelper.FindGameObject(interactGuid);

        MasterHideInCache(interact);
    }

    public bool IsActionForInteractable(IInteractable interact)
    {
        return interact is HideCache
            && interact.GetGameObject.GetComponent<HideCache>().Owner == InteractActor;
    }
}