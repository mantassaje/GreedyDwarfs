using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMode : MonoBehaviour
{
    public bool IsMapMode;
    private Camera DefaultCamera;
    private Camera MapCamera;
    private PhotonView PhotonView;

    private void Awake()
    {
        DefaultCamera = FindObjectOfType<MainCamera>().GetComponent<Camera>();
        MapCamera = FindObjectOfType<MapCamera>().GetComponent<Camera>();
        PhotonView = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (PhotonView.IsMine)
        {
            MapCamera.enabled = IsMapMode;
            DefaultCamera.enabled = !IsMapMode;
        }
    }
}
