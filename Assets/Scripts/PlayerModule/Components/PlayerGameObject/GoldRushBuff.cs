using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GoldRushBuff : MonoBehaviour, IPunObservable
{
    public float GoldRushSpeedAdd = 15;
    public bool IsGoldRushActive = false;
    public float GoldRushSeconds = 8f;

    private float _originalSpeed;

    private Moving _moving;
    private Inventory _inventory;
    private PhotonView _photonView;

    private void Awake()
    {
        _moving = GetComponent<Moving>();
        _inventory = GetComponent<Inventory>();
        _photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        _originalSpeed = _moving.MaxSpeed;
    }

    private void Update()
    {
        if (IsGoldRushActive)
        {
            _moving.MaxSpeed = _originalSpeed + GoldRushSpeedAdd;

            if (!_inventory.HasGold)
            {
                StopGoldRush();
            }
        }
        else
        {
            _moving.MaxSpeed = _originalSpeed;
        }
    }

    public void StartGoldRush()
    {
        CommitStartGoldRush();

        if (PhotonNetwork.IsMasterClient)
        {
            _photonView.RPC(nameof(RpcStartGoldRush), RpcTarget.Others);
        }
    }

    [PunRPC]
    private void RpcStartGoldRush()
    {
        CommitStartGoldRush();
    }

    private void CommitStartGoldRush()
    {
        IsGoldRushActive = true;
        CancelInvoke();
        Invoke(nameof(StopGoldRush), GoldRushSeconds);
    }

    public void StopGoldRush()
    {
        CommitStopGoldRush();

        if (PhotonNetwork.IsMasterClient)
        {
            _photonView.RPC(nameof(RpcStopGoldRush), RpcTarget.Others);
        }
    }

    [PunRPC]
    public void RpcStopGoldRush()
    {
        CommitStopGoldRush();
    }

    public void CommitStopGoldRush()
    {
        IsGoldRushActive = false;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(IsGoldRushActive);
        }
        else
        {
            IsGoldRushActive = (bool)stream.ReceiveNext();
        }
    }
}
