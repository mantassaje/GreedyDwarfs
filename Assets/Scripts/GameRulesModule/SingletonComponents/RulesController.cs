using Photon.Pun;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PhotonView))]
public class RulesController : MonoBehaviour
{
    public int GoldCount;
    public int GoldGoalPerPlayer = 3;
    public int GoldGoal = 3;
    public bool IsGameOver = false;
    public bool IsGoldCollected = false;

    public float RoundTotalSeconds = 60;

    public int HiddenCacheCountForOwner = 2;

    public MultiplayerTimer RoundTimer;

    public PhotonView PhotonView { get; private set; }
    public ScoreSheetController ScoreSheetController { get; private set; }

    private void Awake()
    {
        PhotonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        ScoreSheetController = FindObjectOfType<ScoreSheetController>();
        RoundTimer.OnTimerEnd.AddListener(EndTime);
        RoundTimer.StartTimer(RoundTotalSeconds);

        if (PhotonNetwork.IsMasterClient)
        {
            GoldGoal = PhotonNetwork.CurrentRoom.PlayerCount * GoldGoalPerPlayer;
            Sync();
        }
    }

    public void AddGoldToTotal()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            GoldCount++;

            if (GoldCount >= GoldGoal)
            {
                SetGameOverGoalSuccessScores();
                SetEndGame();
            }

            Sync();
        }
    }

    private void CheckGameOver()
    {
        
    }

    private void EndTime()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            SetGameOverEndTimeScores();
            SetEndGame();
            Sync();
        }
    }

    private void SetEndGame()
    {
        IsGameOver = true;
        RoundTimer.IsRunning = false;
        Debug.Log("Game over. Reloading in 10 seconds");
        Invoke(nameof(ReloadScene), 10);
    }

    public void ReloadScene()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("ReloadScene called.");
            var scene = SceneManager.GetActiveScene();
            PhotonNetwork.LoadLevel("Reload");
        }
    }

    /// <summary>
    /// Gold not collected. Timer ends. Only top player scores.
    /// </summary>
    public void SetGameOverEndTimeScores()
    {
        if (PhotonNetwork.IsMasterClient
            && !IsGameOver)
        {
            IsGoldCollected = false;

            SetStolenLoot();

            if (!ScoreSheetController.ActorScoreSheets.Values.All(sheet => sheet.TotalStolenLootRound == 0))
            {
                var sortedGroup = ScoreSheetController.ActorScoreSheets.Values
                .Where(player => player.TotalStolenLootRound > 0)
                .GroupBy(player => player.TotalStolenLootRound)
                .OrderByDescending(group => group.Key);

                var topPlayersGroup = sortedGroup.First().ToList();

                foreach (var player in topPlayersGroup)
                {
                    player.TotalScore += 5;
                    player.AddToTotalScoreRound = 5;
                }
            }

            ScoreSheetController.SyncAll();
        }
    }

    public void SetGameOverGoalSuccessScores()
    {
        if (PhotonNetwork.IsMasterClient
            && !IsGameOver)
        {
            IsGoldCollected = true;

            SetStolenLoot();

            var players = ScoreSheetController.ActorScoreSheets.Values.ToList();

            var sortedGroup = players
                .GroupBy(player => player.TotalStolenLootRound)
                .OrderByDescending(group => group.Key);

            if (sortedGroup.Count() == 0)
            {
                return;
            }

            var topPlayersGroup = sortedGroup.First().ToList();
            var bottomPlayersGroup = sortedGroup.Last().ToList();

            var midPlayers = players.Except(topPlayersGroup).Except(bottomPlayersGroup);

            foreach (var player in topPlayersGroup)
            {
                player.TotalScore += 2;
                player.AddToTotalScoreRound = 2;
            }

            foreach (var player in midPlayers)
            {
                if (player.AddToTotalScoreRound == 0)
                {
                    player.TotalScore += 1;
                    player.AddToTotalScoreRound = 1;
                }
            }

            ScoreSheetController.SyncAll();
        }
    }

    private void SetStolenLoot()
    {
        ScoreSheetController.ResetRoundData();

        var caches = FindObjectsOfType<HideCache>();

        foreach(var cache in caches)
        {
            if (cache.Owner != null
                && !cache.IsBroken)
            {
                var stolenGold = cache.HiddenGoldCount;

                ScoreSheetController.ActorScoreSheets[cache.Owner.Player.PhotonView.Owner.ActorNumber].TotalStolenLootRound += stolenGold;
            }
        }
    }

    public void SetHiddenCacheOwner(InteractActor player)
    {
        var caches = FindObjectsOfType<HideCache>()
            .Where(cache => cache.Owner == null
                && !cache.IsBroken)
            .ToList();

        for (int i = 0; i < HiddenCacheCountForOwner; i++)
        {
            var cache = caches.PickRandom();
            caches.Remove(cache);

            cache.SetOwner(player);
        }
    }

    private void Sync()
    {
        PhotonView.RPC(nameof(RpcSync), RpcTarget.AllBufferedViaServer, GoldGoal, GoldCount, IsGameOver, IsGoldCollected);
    }

    [PunRPC]
    private void RpcSync(int goldGoal, int goldCount, bool isGameOver, bool isGoldCollected)
    {
        GoldGoal = goldGoal;
        GoldCount = goldCount;
        IsGameOver = isGameOver;
        IsGoldCollected = isGoldCollected;
    }
}
