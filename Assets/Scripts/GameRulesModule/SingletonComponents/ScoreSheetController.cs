using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreSheetController : MonoBehaviourPunCallbacks, IPunObservable
{
    public class ActorScoreSheet
    {
        public int ActorNumber { get; set; }
        public int TotalScore { get; set; }
        public int AddToTotalScoreRound { get; set; }
        public int TotalStolenLootRound { get; set; }
        public int TotalGoldCollectedRound { get; set; }
    }

    public PhotonView PhotonView { get; private set; }
    public Dictionary<int, ActorScoreSheet> ActorScoreSheets { get; private set; } = new Dictionary<int, ActorScoreSheet>();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        PhotonView = GetComponent<PhotonView>();
    }

    public void RemoveSheet(Photon.Realtime.Player player)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            ActorScoreSheets.TryGetValue(player.ActorNumber, out var sheet);

            if (sheet != null)
            {
                ActorScoreSheets.Remove(player.ActorNumber);
            }
        }
    }

    public void UpdateScoreSheet(Photon.Realtime.Player player, int totalScore, int addToTotalScoreRound, int totalStolenLootRound)
    {
        ActorScoreSheets.TryGetValue(player.ActorNumber, out var sheet);

        if (sheet != null)
        {
            sheet.TotalScore = totalScore;
            sheet.AddToTotalScoreRound = addToTotalScoreRound;
            sheet.TotalStolenLootRound = totalStolenLootRound;
        }
        else
        {
            ActorScoreSheets[player.ActorNumber] = new ActorScoreSheet()
            {
                ActorNumber = player.ActorNumber,
                TotalScore = totalScore,
                AddToTotalScoreRound = addToTotalScoreRound,
                TotalStolenLootRound = totalStolenLootRound
            };
        }
    }

    public List<Photon.Realtime.Player> GetHighScorePlayers()
    {
        var scoreGroups = ActorScoreSheets.Values
            .GroupBy(sheet => sheet.TotalScore)
            .OrderByDescending(sheets => sheets.Key);

        return scoreGroups
            .FirstOrDefault()
            .Select(sheet => PhotonNetwork.CurrentRoom.Players.ContainsKey(sheet.ActorNumber)
                ? PhotonNetwork.CurrentRoom.Players[sheet.ActorNumber]
                : null)
            .Where(player => player != null)
            .ToList();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(ActorScoreSheets.Count);

            foreach(var sheet in ActorScoreSheets.Values)
            {
                stream.SendNext(sheet.ActorNumber);
                stream.SendNext(sheet.AddToTotalScoreRound);
                stream.SendNext(sheet.TotalScore);
                stream.SendNext(sheet.TotalStolenLootRound);
                stream.SendNext(sheet.TotalGoldCollectedRound);
            }
        }
        else
        {
            var count = (int)stream.ReceiveNext();

            ActorScoreSheets = new Dictionary<int, ActorScoreSheet>();

            for (int i = 0; i < count; i++)
            {
                var sheet = new ActorScoreSheet();
                sheet.ActorNumber = (int)stream.ReceiveNext();
                sheet.AddToTotalScoreRound = (int)stream.ReceiveNext();
                sheet.TotalScore = (int)stream.ReceiveNext();
                sheet.TotalStolenLootRound = (int)stream.ReceiveNext();
                sheet.TotalGoldCollectedRound = (int)stream.ReceiveNext();

                ActorScoreSheets[sheet.ActorNumber] = sheet;
            }
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
