using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PhotonView))]
public class Moving : MonoBehaviour, IPunObservable
{
    public float MaxSpeed = 25;

    public Rigidbody2D Body { get; private set; }
    public PhotonView PhotonView { get; private set; }

    /// <summary>
    /// Used by other players that do not controll this object.
    /// </summary>
    private Vector2? _velocityGuess;

    private void Awake()
    {
        Body = GetComponent<Rigidbody2D>();
        PhotonView = GetComponent<PhotonView>();
    }

    /// <summary>
    /// Call in  FixedUpdate.
    /// </summary>
    public void MoveRight()
    {
        Rigidbody2DHelper.MoveRight(Body, MaxSpeed);
    }

    /// <summary>
    /// Call in  FixedUpdate.
    /// </summary>
    public void MoveLeft()
    {
        Rigidbody2DHelper.MoveLeft(Body, MaxSpeed);
    }

    /// <summary>
    /// Call in  FixedUpdate.
    /// </summary>
    public void MoveTop()
    {
        Rigidbody2DHelper.MoveTop(Body, MaxSpeed);
    }

    /// <summary>
    /// Call in  FixedUpdate.
    /// </summary>
    public void MoveBottom()
    {
        Rigidbody2DHelper.MoveBottom(Body, MaxSpeed);
    }

    private void FixedUpdate()
    {
        if (!PhotonView.IsMine
            && _velocityGuess.HasValue)
        {
            Body.velocity = _velocityGuess.Value;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(MaxSpeed);
            stream.SendNext(Body.velocity.x);
            stream.SendNext(Body.velocity.y);
        }
        else
        {
            MaxSpeed = (float)stream.ReceiveNext();
            var x = (float)stream.ReceiveNext();
            var y = (float)stream.ReceiveNext();

            _velocityGuess = new Vector2(x, y);
            Body.velocity = _velocityGuess.Value;
        }
    }
}
