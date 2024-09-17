using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class Cavein : MonoBehaviour, IPunObservable
{
    public PhotonView PhotonView { get; private set; }
    public SpriteRenderer SpriteRenderer { get; private set; }
    public Collider2D Collider2D { get; private set; }

    public bool _isClear = false;

    private void Awake()
    {
        PhotonView = GetComponent<PhotonView>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        Collider2D = GetComponent<Collider2D>();
    }

    public void Destroy()
    {
        _isClear = true;
    }

    private void Update()
    {
        if (_isClear
            && SpriteRenderer != null
            && SpriteRenderer.enabled)
        {
            Destroy(SpriteRenderer);
            Destroy(Collider2D);

            SpriteRenderer = null;
            Collider2D = null;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_isClear);
        }
        else
        {
            _isClear = (bool)stream.ReceiveNext();
        }
    }
}
