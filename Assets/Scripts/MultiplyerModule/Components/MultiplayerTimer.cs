using Photon.Pun;
using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PhotonView))]
public class MultiplayerTimer : MonoBehaviour, IPunObservable
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
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(SecondsRemaining);
        }
        else
        {
            SecondsRemaining = (float)stream.ReceiveNext();
        }
    }

}
