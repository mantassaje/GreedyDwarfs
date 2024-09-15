using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PhotonView PhotonView { get; private set; }
    private NotificationText _notificationText;
    private bool _areCachesAssigned = false;

    private void Awake()
    {
        PhotonView = GetComponent<PhotonView>();
    }

    public void Start()
    {
        Debug.Log($"PhotonView.Owner.UserId {PhotonView.Owner.UserId}.");

        _notificationText = FindObjectOfType<NotificationText>();
    }

    private void Update()
    {
        if (!_areCachesAssigned
            && PhotonNetwork.IsMasterClient)
        {
            FindObjectOfType<RulesController>().SetHiddenCacheOwner(GetComponent<InteractActor>());
            _areCachesAssigned = true;
            Sync();
        }
    }

    public void Sync()
    {
        PhotonView.RPC(nameof(RpcSync), RpcTarget.AllBufferedViaServer, _areCachesAssigned);
    }

    [PunRPC]
    private void RpcSync(bool areCachesAssigned)
    {
        _areCachesAssigned = areCachesAssigned;
    }

    public void Notify(string message)
    {
        PhotonView.RPC(nameof(RpcNotify), RpcTarget.All, message);
    }

    [PunRPC]
    private void RpcNotify(string message)
    {
        if (PhotonView.IsMine)
        {
            _notificationText.Notify(message);
        }
    }
}
