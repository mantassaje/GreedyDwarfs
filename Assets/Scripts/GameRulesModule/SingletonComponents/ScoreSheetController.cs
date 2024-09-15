using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreSheetController : MonoBehaviourPunCallbacks
{
    public class ActorScoreSheet
    {
        public int ActorNumber { get; set; }
        public int TotalScore { get; set; }
        public int AddToTotalScoreRound { get; set; }
        public int TotalStolenLootRound { get; set; }
    }

    public PhotonView PhotonView { get; private set; }
    public Dictionary<int, ActorScoreSheet> ActorScoreSheets { get; private set; } = new Dictionary<int, ActorScoreSheet>();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        PhotonView = GetComponent<PhotonView>();
    }

    public void ResetRoundData()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            foreach (var sheet in ActorScoreSheets)
            {
                sheet.Value.AddToTotalScoreRound = 0;
                sheet.Value.TotalStolenLootRound = 0;

                PhotonView.RPC(nameof(RpcUpdateScoreSheet), RpcTarget.AllBufferedViaServer, sheet.Value.ActorNumber, sheet.Value.TotalScore, 0, 0);
            }
        }
    }

    public void SyncAll()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            foreach (var sheet in ActorScoreSheets)
            {
                PhotonView.RPC(
                    nameof(RpcUpdateScoreSheet), 
                    RpcTarget.AllBufferedViaServer,
                    sheet.Value.ActorNumber,
                    sheet.Value.TotalScore,
                    sheet.Value.AddToTotalScoreRound,
                    sheet.Value.TotalStolenLootRound
                );
            }
        }
    }

    public void UpdateScoreSheet(Photon.Realtime.Player player, int totalScore, int addToTotalScoreRound, int totalStolenLootRound)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonView.RPC(nameof(RpcUpdateScoreSheet), RpcTarget.AllBufferedViaServer, player.ActorNumber, totalScore, addToTotalScoreRound, totalStolenLootRound);

            RefreshActorScoreSheet(player.ActorNumber, totalScore, addToTotalScoreRound, totalStolenLootRound);
        }
    }

    [PunRPC]
    private void RpcUpdateScoreSheet(int actorNumber, int totalScore, int addToTotalScoreRound, int totalStolenLootRound)
    {
        RefreshActorScoreSheet(actorNumber, totalScore, addToTotalScoreRound, totalStolenLootRound);
    }

    public void RemoveSheet(Photon.Realtime.Player player)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonView.RPC(nameof(RpcRemoveSheet), RpcTarget.AllBufferedViaServer, player.ActorNumber);

            CommitRemove(player.ActorNumber);
        }
    }

    [PunRPC]
    private void RpcRemoveSheet(int actorNumber)
    {
        CommitRemove(actorNumber);
    }

    private void CommitRemove(int actorNumber)
    {
        ActorScoreSheets.TryGetValue(actorNumber, out var sheet);

        if (sheet != null)
        {
            ActorScoreSheets.Remove(actorNumber);
        }
    }

    private void RefreshActorScoreSheet(int actorNumber, int totalScore, int addToTotalScoreRound, int totalStolenLootRound)
    {
        ActorScoreSheets.TryGetValue(actorNumber, out var sheet);

        if (sheet != null)
        {
            sheet.TotalScore = totalScore;
            sheet.AddToTotalScoreRound = addToTotalScoreRound;
            sheet.TotalStolenLootRound = totalStolenLootRound;
        }
        else
        {
            ActorScoreSheets[actorNumber] = new ActorScoreSheet()
            {
                ActorNumber = actorNumber,
                TotalScore = totalScore,
                AddToTotalScoreRound = addToTotalScoreRound,
                TotalStolenLootRound = totalStolenLootRound
            };
        }
    }

    public override void OnLeftRoom()
    {

    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        Debug.Log("OnPlayerLeftRoom called in ScoreSheetController.");
        RemoveSheet(otherPlayer);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("OnPlayerEnteredRoom called in ScoreSheetController.");
        UpdateScoreSheet(PhotonNetwork.LocalPlayer, 0, 0, 0);
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log("OnPlayerEnteredRoom called in ScoreSheetController.");
        UpdateScoreSheet(newPlayer, 0, 0, 0);
    }
}
