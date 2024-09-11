using Photon.Pun;
using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PhotonView))]
public class MultiplayerTimer : MonoBehaviour
{
    private PhotonView PhotonView;

    public float SecondsRemaining = 10;
    public bool IsRunning { get; private set; }

    private int _syncEveryTick = 1000;
    private int _leftTick;

    public UnityEvent OnTimerEnd = new UnityEvent();

    private void Awake()
    {
        PhotonView = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (IsRunning)
        {
            if (_leftTick-- < 0
                && PhotonNetwork.IsMasterClient)
            {
                _leftTick = _syncEveryTick;
                Sync();
            }

            if (SecondsRemaining > 0)
            {
                SecondsRemaining -= Time.deltaTime;
            }
            else
            {
                Debug.Log("Time has run out!");
                SecondsRemaining = 0;
                IsRunning = false;
                OnTimerEnd.Invoke();
            }
        }
    }

    public void StartTimer(float seconds)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            IsRunning = true;
            SecondsRemaining = seconds;
            Sync();
        }
    }

    public void Sync()
    {
        PhotonView.RPC(nameof(Sync), RpcTarget.AllBufferedViaServer, SecondsRemaining, IsRunning);
    }

    [PunRPC]
    private void Sync(float secondsRemaining, bool isRunning)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            SecondsRemaining = secondsRemaining;
            IsRunning = isRunning;
        }
    }

}
