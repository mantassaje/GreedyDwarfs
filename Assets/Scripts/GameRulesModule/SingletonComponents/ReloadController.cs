using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadController : MonoBehaviour
{
    void Start()
    {
        //Hack
        Invoke(nameof(Reload), 2);
    }

    private void Reload()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("Game");
        }
    }
}
