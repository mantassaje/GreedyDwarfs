using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class PlaceInCargo : MonoBehaviour, IInteractActorAction
{
    public Inventory Inventory { get; private set; }
    public InteractActor InteractActor { get; private set; }
    public PlayerToken PlayerToken { get; private set; }
    public PhotonView PhotonView { get; private set; }

    void Awake()
    {
        Inventory = GetComponent<Inventory>();
        InteractActor = GetComponent<InteractActor>();
        PlayerToken = GetComponent<PlayerToken>();
        PhotonView = GetComponent<PhotonView>();
    }

    public bool Act(InteractActor actor, GameObject interact)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            return MasterPlaceInCargo(interact);
        }
        else
        {
            if (!IsValidAction(interact))
            {
                return false;
            }

            PhotonView.RPC(nameof(RpcPlaceInCargo), RpcTarget.MasterClient, PhotonViewIdHelper.GetId(interact));

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

    private bool MasterPlaceInCargo(GameObject interact)
    {
        if (!IsValidAction(interact))
        {
            return false;
        }

        if (interact.GetComponent<IInteractable>().Interact(InteractActor))
        {
            Inventory.RemoveGold();
            PlayerToken.TotalGoldCollected++;
            GetComponent<PlayerToken>().Notify("Item loaded into cargo wagon", false);
            return true;
        }

        return false;
    }

    [PunRPC]
    private void RpcPlaceInCargo(int interactGuid)
    {
        var interact = PhotonViewIdHelper.FindGameObject(interactGuid);

        MasterPlaceInCargo(interact);
    }

    public bool IsActionForInteractable(IInteractable interact)
    {
        return interact is Cargo;
    }
}