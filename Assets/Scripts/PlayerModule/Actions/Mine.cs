
using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
[RequireComponent(typeof(Blink))]
[RequireComponent(typeof(InteractActor))]
[RequireComponent(typeof(PhotonView))]
public class Mine : MonoBehaviour, IInteractActorAction
{
    public int MineCooldownLeft;
    public int TotalMineCooldown = 1000;

    public Inventory Inventory { get; private set; }
    public Blink Blink { get; private set; }
    public InteractActor InteractActor { get; private set; }
    public PhotonView PhotonView { get; private set; }

    private GoldRushBuff GoldRushBuff;

    void Awake()
    {
        Inventory = GetComponent<Inventory>();
        Blink = GetComponent<Blink>();
        InteractActor = GetComponent<InteractActor>();
        GoldRushBuff = GetComponent<GoldRushBuff>();
        PhotonView = GetComponent<PhotonView>();
    }

    void FixedUpdate()
    {
        MineCooldownLeft--;

        MineCooldownLeft = MathHelper.Bound(MineCooldownLeft, 0, TotalMineCooldown);
    }

    public bool Act(InteractActor actor, GameObject interact)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            return MasterMine(interact);
        }
        else
        {
            if (!IsValidAction(interact))
            {
                return false;
            }

            PhotonView.RPC(nameof(RpcMine), RpcTarget.MasterClient, PhotonViewIdHelper.GetId(interact));

            return true;
        }
        
    }

    private bool IsValidAction(GameObject interact)
    {
        if (MineCooldownLeft > 0
            || Inventory.HasGold)
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

    private bool MasterMine(GameObject interact)
    {
        if (!IsValidAction(interact))
        {
            return false;
        }

        MineCooldownLeft = TotalMineCooldown;

        if (interact.GetComponent<IInteractable>().Interact(InteractActor))
        {
            Inventory.AddGold();
            GoldRushBuff.StartGoldRush();
            Blink.BlinkOnce();
        }

        Sync();

        return true;
    }

    public bool IsActionForInteractable(IInteractable interact)
    {
        return interact is Mineable;
    }

    private void Sync()
    {
        PhotonView.RPC(nameof(RpcSync), RpcTarget.Others, MineCooldownLeft);
    }

    [PunRPC]
    private void RpcSync(int mineCooldownLeft)
    {
        MineCooldownLeft = mineCooldownLeft;
    }

    [PunRPC]
    private void RpcMine(int interactGuid)
    {
        var interact = PhotonViewIdHelper.FindGameObject(interactGuid);

        MasterMine(interact);
    }
}