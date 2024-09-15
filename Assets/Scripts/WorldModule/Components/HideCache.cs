using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(PhotonView))]
public class HideCache : MonoBehaviour, IInteractable
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
        PhotonView.RPC(
            nameof(RpcSetOwner), 
            RpcTarget.AllBufferedViaServer, 
            GuidReferenceHelper.GetId(owner).ToString()
        );
    }

    [PunRPC]
    private void RpcSetOwner(string actorGuid)
    {
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
        Sync();

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
        Sync();

        return true;
    }

    private void DrawBrake()
    {
        SpriteRenderer.color = new Color(0.2f, 0.2f, 0.2f, 0.5f);
    }

    private void Sync()
    {
        PhotonView.RPC(nameof(RpcSync), RpcTarget.AllBufferedViaServer, HiddenGoldCount, IsBroken);
    }

    [PunRPC]
    private void RpcSync(int hiddenGoldCount, bool isBroken)
    {
        HiddenGoldCount = hiddenGoldCount;
        IsBroken = isBroken;

        if (isBroken)
        {
            DrawBrake();
        }
    }
}
