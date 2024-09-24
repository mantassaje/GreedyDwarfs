using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class BreakCache : MonoBehaviour, IInteractActorAction
{
    public Inventory Inventory { get; private set; }
    public Player Player { get; private set; }
    public InteractActor InteractActor { get; private set; }
    public PhotonView PhotonView { get; private set; }

    public float TotalCooldownSeconds = 30;
    public MultiplayerTimer CooldownTimer;

    void Awake()
    {
        Inventory = GetComponent<Inventory>();
        Player = GetComponent<Player>();
        InteractActor = GetComponent<InteractActor>();
        PhotonView = GetComponent<PhotonView>();
    }

    public bool Act(InteractActor actor, GameObject interact)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            return MasterBreakCache(interact);
        }
        else
        {
            if (!IsValidAction(interact))
            {
                if (CooldownTimer.IsRunning)
                {
                    Player.Notify("Wait for cooldown", false);
                }

                return false;
            }

            PhotonView.RPC(nameof(RpcDoBreakCache), RpcTarget.MasterClient, PhotonViewIdHelper.GetId(interact));

            return true;
        }
    }

    private bool IsValidAction(GameObject interact)
    {
        var distance = Vector2.Distance(InteractActor.transform.position, interact.transform.position);

        if (distance > 2f)
        {
            return false;
        }

        if (CooldownTimer.IsRunning)
        {
            return false;
        }

        return true;
    }

    private bool MasterBreakCache(GameObject interact)
    {
        if (!IsValidAction(interact))
        {
            if (CooldownTimer.IsRunning)
            {
                Player.Notify("Wait for cooldown", false);
            }

            return false;
        }

        if (interact.GetComponent<IInteractable>().Interact(InteractActor))
        {
            CooldownTimer.StartTimer(TotalCooldownSeconds);

            var hideCache = interact.GetComponent<HideCache>();
            if (hideCache.IsBroken)
            {
                Player.Notify("You found stolen loot!", true);
                //Null checked in case player left mid game.
                hideCache.Owner?.Player?.Notify("Your stolen loot was found!", true);
                Inventory.AddGold();
            }
            else
            {
                Player.Notify("Nothing found", true);
            }

            return true;
        }

        return false;
    }

    [PunRPC]
    private void RpcDoBreakCache(int interactGuid)
    {
        var interact = PhotonViewIdHelper.FindGameObject(interactGuid);

        MasterBreakCache(interact);
    }

    public bool IsActionForInteractable(IInteractable interact)
    {
        return interact is HideCache
            && interact.GetGameObject.GetComponent<HideCache>().Owner != InteractActor;
    }
}