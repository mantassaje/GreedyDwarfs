using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Blink))]
public class Mineable : MonoBehaviour, IInteractable
{
    private Blink Blink { get; set; }
    private PhotonView PhotonView { get; set; }

    public int MaxHitsAdd = 5;
    public int MaxGoldAdd = 5;
    public int MinHits = 3;
    public int MinGold = 3;

    //[ReadOnly]
    public int GoldCount;

    //[ReadOnly]
    public int HitCountLeft;

    void Awake()
    {
        Blink = GetComponent<Blink>();
        PhotonView = GetComponent<PhotonView>();

        GoldCount = UnityEngine.Random.Range(1, MaxGoldAdd + 1) + MinGold;
        GenerateHits();
    }

    private void GenerateHits()
    {
        HitCountLeft = UnityEngine.Random.Range(1, MaxHitsAdd + 1) + MinHits;
    }

    private void ConsumeGold()
    {
        GoldCount--;
        GenerateHits();

        if (GoldCount <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    public bool Interact(InteractActor actor)
    {
        Blink.BlinkOnce();

        HitCountLeft--;

        if (HitCountLeft <= 0)
        {
            ConsumeGold();
            Sync();
            return true;
        }
        else
        {
            Sync();
            return false;
        }
    }

    public void Sync()
    {
        PhotonView.RPC(nameof(RpcSync), RpcTarget.AllBufferedViaServer, GoldCount, HitCountLeft);
    }

    [PunRPC]
    private void RpcSync(int goldCount, int hitCount)
    {
        GoldCount = goldCount;
        HitCountLeft = hitCount;

        if (GoldCount <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
