using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Blink : MonoBehaviour
{
    private Color? OriginalColor;
    public Color BlinkColor;
    public float BlinkHoldSeconds = 0.5f;
    public bool SyncMultiplayer = true;

    private bool _repeatBlink = false;

    public SpriteRenderer SpriteRenderer { get; private set; }
    public PhotonView PhotonView { get; private set; }


    void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        PhotonView = GetComponent<PhotonView>();
    }

    private void DoBlink()
    {
        if (OriginalColor == null)
        {
            OriginalColor = SpriteRenderer.color;
        }

        SpriteRenderer.color = BlinkColor;
    }

    public void BlinkOnce()
    {
        if (SyncMultiplayer
            && PhotonNetwork.IsMasterClient)
        {
            PhotonView.RPC(nameof(RpcBlinkOnce), RpcTarget.Others);
        }

        _repeatBlink = false;

        DoBlink();

        CancelInvoke();
        Invoke(nameof(ResetColor), BlinkHoldSeconds);
    }

    public void BlinkRepeated()
    {
        if (SyncMultiplayer
            && PhotonNetwork.IsMasterClient)
        {
            PhotonView.RPC(nameof(RpcBlinkRepeated), RpcTarget.Others);
        }

        _repeatBlink = true;

        DoBlink();

        CancelInvoke();
        Invoke(nameof(ResetColor), BlinkHoldSeconds);
    }

    private void ResetColor()
    {
        SpriteRenderer.color = OriginalColor.Value;

        if (_repeatBlink)
        {
            Invoke(nameof(BlinkRepeated), BlinkHoldSeconds);
        }
    }

    [PunRPC]
    private void RpcBlinkOnce()
    {
        BlinkOnce();
    }

    [PunRPC]
    private void RpcBlinkRepeated()
    {
        BlinkRepeated();
    }
}
