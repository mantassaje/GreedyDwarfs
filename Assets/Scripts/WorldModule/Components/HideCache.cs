using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(PhotonView))]
public class HideCache : MonoBehaviourPunCallbacks, IInteractable, IPunObservable
{
    private Blink Blink;
    public InteractActor Owner;

    public bool IsBroken;
    public int HiddenGoldCount;
    public Blink HighlightForOwner;

    public SpriteRenderer SpriteRenderer { get; private set; }
    public PhotonView PhotonView { get; private set; }

    public GameObject GetGameObject => this.gameObject;

    void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        PhotonView = GetComponent<PhotonView>();
        Blink = GetComponent<Blink>();
    }

    private void Start()
    {
        HighlightForOwner.gameObject.SetActive(false);
    }

    public void SetOwner(InteractActor owner)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonView.RPC(
                nameof(RpcSetOwner),
                RpcTarget.All,
                GuidReferenceHelper.GetId(owner).ToString()
            );
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonView.RPC(
                nameof(RpcSetOwner),
                RpcTarget.All,
                GuidReferenceHelper.GetId(Owner).ToString()
            );
        }
    }

    [PunRPC]
    private void RpcSetOwner(string actorGuid)
    {
        if (Owner != null)
        {
            return;
        }

        var owner = GuidReferenceHelper.FindGameObject<InteractActor>(actorGuid);

        Owner = owner;

        if (Owner.PhotonView.IsMine)
        {
            HighlightForOwner.gameObject.SetActive(true);
            HighlightForOwner.BlinkRepeated();
        }
    }

    public bool Interact(InteractActor actor)
    {
        if (IsBroken)
        {
            return false;
        }

        if (Owner == actor)
        {
            return Steal(actor);
        }
        else
        {
            return TryBreakCache(actor);
        }
        
    }

    private bool Steal(InteractActor actor)
    {
        HiddenGoldCount++;

        Blink.BlinkOnce();

        return true;
    }

    private bool TryBreakCache(InteractActor actor)
    {
        if (HiddenGoldCount <= 0)
        {
            return true;
        }

        IsBroken = true;
        HiddenGoldCount = 0;
        DrawBrake();

        return true;
    }

    private void DrawBrake()
    {
        SpriteRenderer.color = new Color(0.2f, 0.2f, 0.2f, 0.2f);
        HighlightForOwner.gameObject.SetActive(false);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(HiddenGoldCount);
            stream.SendNext(IsBroken);
        }
        else
        {
            HiddenGoldCount = (int)stream.ReceiveNext();
            var isBrokenNew = (bool)stream.ReceiveNext();

            if (!IsBroken
                && isBrokenNew)
            {
                DrawBrake();
            }

            IsBroken = isBrokenNew;
        }
    }
}
