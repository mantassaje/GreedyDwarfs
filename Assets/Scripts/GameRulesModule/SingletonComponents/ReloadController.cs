using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadController : MonoBehaviour
{
    void Start()
    {
        PhotonNetwork.LoadLevel("Game");
    }
}
