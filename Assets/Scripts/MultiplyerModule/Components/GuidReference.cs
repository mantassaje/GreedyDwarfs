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

        GenerateGuidIfNull();
    }

    public override void OnJoinedRoom()
    {
        GenerateGuidIfNull();
    }

    private void GenerateGuidIfNull()
    {
        if (PhotonNetwork.IsMasterClient
            && Id == Guid.Empty)
        {
            Id = Guid.NewGuid();
            DebugIdShow = Id.ToString();

            PhotonView.RPC(nameof(RpcSyncGuid), RpcTarget.AllBufferedViaServer, Id.ToString());
        }
    }

    [PunRPC]
    private void RpcSyncGuid(string guidId)
    {
        Id = new Guid(guidId);
        DebugIdShow = Id.ToString();
    }
}
