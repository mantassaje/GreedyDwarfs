using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerData PlayerData { get; private set; }
    public PhotonView PhotonView { get; private set; }
    private NotificationText _notificationText;

    private void Awake()
    {
        PhotonView = GetComponent<PhotonView>();
    }

    public void Start()
    {
        Debug.Log($"PhotonView.Owner.UserId {PhotonView.Owner.UserId}.");

        /*PlayerData = FindObjectsOfType<PlayerData>()
            .ToList()
            .First(data => data.PhotonView.Owner.UserId == PhotonView.Owner.UserId);*/

        _notificationText = FindObjectOfType<NotificationText>();
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

    public void SetPlayerData(PlayerData playerData)
    {
        var guid = GuidReferenceHelper.GetId(playerData).ToString();
        PlayerData = playerData;
        PhotonView.RPC(nameof(RpcSetPlayerData), RpcTarget.AllBufferedViaServer, guid);
    }

    [PunRPC]
    private void RpcSetPlayerData(string guid)
    {
        var playerData = GuidReferenceHelper.FindGameObject<PlayerData>(guid);
        PlayerData = playerData;
    }
}
