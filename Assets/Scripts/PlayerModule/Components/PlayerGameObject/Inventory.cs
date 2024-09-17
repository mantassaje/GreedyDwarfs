using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour, IPunObservable
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
            PhotonView.RPC(nameof(RcpAddGold), RpcTarget.Others);
        }
    }

    public void RemoveGold()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _hasGold = false;
            PhotonView.RPC(nameof(RpcRemoveGold), RpcTarget.Others);
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_hasGold);
        }
        else if (!PhotonNetwork.IsMasterClient)
        {
            _hasGold = (bool)stream.ReceiveNext();
        }
    }
}
