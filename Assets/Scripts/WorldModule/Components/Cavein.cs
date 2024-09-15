using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class Cavein : MonoBehaviour
{
    public PhotonView PhotonView { get; private set; }

    private void Awake()
    {
        PhotonView = GetComponent<PhotonView>();
    }

    public void Destroy()
    {
        PhotonView.RPC(nameof(RpcDestry), RpcTarget.AllBufferedViaServer);
    }

    [PunRPC]
    private void RpcDestry()
    {
        gameObject.SetActive(false);
    }
}
