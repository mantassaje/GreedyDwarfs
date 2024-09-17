using Photon.Pun;
using System;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class GuidReference : MonoBehaviourPunCallbacks, IPunObservable
{
    public Guid Id { get; private set; }

    private PhotonView PhotonView;

    //[ReadOnly]
    public string DebugIdShow;

    public GuidReference()
    {
        
    }

    private void Update()
    {
        //Would be good to log empty guids but for clieants later when the guids are sent.
        /*if (Id == Guid.Empty)
        {
            Debug.LogError($"Guid is {Id}.", this);
        }*/
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(Id.ToString());
        }
        else
        {
            var id = (string)stream.ReceiveNext();

            if (id == Guid.Empty.ToString())
            {
                return;
            }

            Id = new Guid(id);

            DebugIdShow = id;
        }
    }
}
