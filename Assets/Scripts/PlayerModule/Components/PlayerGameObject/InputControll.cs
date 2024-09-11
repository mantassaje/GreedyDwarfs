using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Moving))]
[RequireComponent(typeof(InteractActor))]
[RequireComponent(typeof(MapMode))]
[RequireComponent(typeof(PhotonView))]
public class InputControll : MonoBehaviour
{
    public Moving Moving { get; private set; }
    public InteractActor InteractActor { get; private set; }
    public MapMode MapMode { get; private set; }
    public PhotonView PhotonView { get; private set; }

    private void Awake()
    {
        Moving = GetComponent<Moving>();
        InteractActor = GetComponent<InteractActor>();
        MapMode = GetComponent<MapMode>();
        PhotonView = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (!PhotonView.IsMine)
        {
            return;
        }
        if (Input.GetKey(KeyCode.Tab))
        {
            MapMode.IsMapMode = true;
            return;
        }
        else
        {
            MapMode.IsMapMode = false;
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            InteractActor.InteractWithObjectInMousePosition();
        }
    }

    private void FixedUpdate()
    {
        if (!PhotonView.IsMine)
        {
            return;
        }

        if (MapMode.IsMapMode)
        {
            return;
        }

        if (Input.GetKey(KeyCode.W))
        {
            Moving.MoveTop();
        }

        if (Input.GetKey(KeyCode.S))
        {
            Moving.MoveBottom();
        }

        if (Input.GetKey(KeyCode.A))
        {
            Moving.MoveLeft();
        }

        if (Input.GetKey(KeyCode.D))
        {
            Moving.MoveRight();
        }
    }
}
