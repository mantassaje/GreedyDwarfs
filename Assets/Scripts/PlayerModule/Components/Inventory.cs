using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private bool _hasGold;
    public GameObject HoldVisual;
    public PhotonView PhotonView { get; private set; }
    public bool HasGold { get => _hasGold; }

    private void Awake()
    {
        PhotonView = GetComponent<PhotonView>();
    }

    private void Update()
    {
        HoldVisual.SetActive(_hasGold);
    }

    public void AddGold()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _hasGold = true;
            PhotonView.RPC(nameof(RcpAddGold), RpcTarget.AllBufferedViaServer);
        }
    }

    public void RemoveGold()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _hasGold = false;
            PhotonView.RPC(nameof(RpcRemoveGold), RpcTarget.AllBufferedViaServer);
        }
    }

    [PunRPC]
    private void RcpAddGold()
    {
        _hasGold = true;
    }

    [PunRPC]
    private void RpcRemoveGold()
    {
        _hasGold = false;
    }
}
