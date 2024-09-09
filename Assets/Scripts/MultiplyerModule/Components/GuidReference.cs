using Photon.Pun;
using System;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class GuidReference : MonoBehaviourPunCallbacks
{
    public Guid Id { get; private set; }

    private PhotonView PhotonView;

    //[ReadOnly]
    public string DebugIdShow;

    public GuidReference()
    {
        
    }

    private void Awake()
    {
        PhotonView = GetComponent<PhotonView>();

        if (PhotonNetwork.IsMasterClient)
        {
            Id = Guid.NewGuid();
            DebugIdShow = Id.ToString();

            PhotonView.RPC(nameof(RpcSync), RpcTarget.AllBufferedViaServer, Id.ToString());
        }
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Id = Guid.NewGuid();
            DebugIdShow = Id.ToString();

            PhotonView.RPC(nameof(RpcSync), RpcTarget.AllBufferedViaServer, Id.ToString());
        }
    }

    [PunRPC]
    private void RpcSync(string guidId)
    {
        Id = new Guid(guidId);
        DebugIdShow = Id.ToString();
    }
}
