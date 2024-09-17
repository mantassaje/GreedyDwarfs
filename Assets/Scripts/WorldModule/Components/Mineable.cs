using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mineable : MonoBehaviour, IInteractable, IPunObservable
{
    private Blink Blink { get; set; }
    private PhotonView PhotonView { get; set; }
    private SpriteRenderer SpriteRenderer { get; set; }
    private Collider2D Collider2D { get; set; }
    public GameObject GetGameObject => this.gameObject;

    public int MaxHitsAdd = 5;
    public int MaxGoldAdd = 5;
    public int MinHits = 3;
    public int MinGold = 3;

    public int GoldCount;
    public int HitCountLeft;

    void Awake()
    {
        Blink = GetComponent<Blink>();
        PhotonView = GetComponent<PhotonView>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        Collider2D = GetComponent<Collider2D>();

        GoldCount = UnityEngine.Random.Range(1, MaxGoldAdd + 1) + MinGold;
        GenerateHits();
    }

    private void GenerateHits()
    {
        HitCountLeft = UnityEngine.Random.Range(1, MaxHitsAdd + 1) + MinHits;
    }

    private void ConsumeGold()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            GoldCount--;
            GenerateHits();
        }
    }

    private void Update()
    {
        var isMined = GoldCount <= 0;

        if (isMined
            && SpriteRenderer != null
            && SpriteRenderer.enabled)
        {
            Destroy(Blink);
            Destroy(SpriteRenderer);
            Destroy(Collider2D);

            Blink = null;
            SpriteRenderer = null;
            Collider2D = null;
        }
    }

    public bool Interact(InteractActor actor)
    {
        if (GoldCount <= 0)
        {
            return false;
        }

        Blink.BlinkOnce();

        HitCountLeft--;

        if (HitCountLeft <= 0)
        {
            ConsumeGold();
            return true;
        }
        else
        {
            return false;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(GoldCount);
            stream.SendNext(HitCountLeft);
        }
        else
        {
            GoldCount = (int)stream.ReceiveNext();
            HitCountLeft = (int)stream.ReceiveNext();
        }
    }
}
