using Photon.Pun;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PhotonView))]
public class RulesController : MonoBehaviour
{
    public int GoldCount;
    public int GoldGoal = 3;
    public bool IsGameOver = false;
    public bool IsGoldCollected = false;

    public float RoundTotalSeconds = 60;

    public int HiddenCacheCountForOwner = 2;

    public MultiplayerTimer RoundTimer;

    public PhotonView PhotonView { get; private set; }

    private void Awake()
    {
        PhotonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        RoundTimer.OnTimerEnd.AddListener(EndTime);
        RoundTimer.StartTimer(RoundTotalSeconds);
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
        Debug.Log("Game over. Reloading in 5 seconds");
        Invoke(nameof(ReloadScene), 5);
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
        if (PhotonNetwork.IsMasterClient)
        {
            IsGoldCollected = false;

            SetStolenLoot();

            var players = FindObjectsOfType<PlayerData>();

            var sortedGroup = players
                .Where(player => player.TotalStolenLootRound > 0)
                .GroupBy(player => player.TotalStolenLootRound)
                .OrderByDescending(group => group.Key);

            var topPlayersGroup = sortedGroup.First().ToList();

            foreach(var player in topPlayersGroup)
            {
                player.AddToTotalScore(5);
            }
        }
    }

    public void SetGameOverGoalSuccessScores()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            IsGoldCollected = true;

            SetStolenLoot();

            var players = FindObjectsOfType<PlayerData>();

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
                player.AddToTotalScore(2);
            }

            foreach (var player in midPlayers)
            {
                if (player.AddToTotalScoreRound == 0)
                {
                    player.AddToTotalScore(1);
                }
            }
        }
    }

    private void SetStolenLoot()
    {
        var players = FindObjectsOfType<PlayerData>();

        foreach(var player in players)
        {
            player.TotalStolenLootRound = 0;
            player.AddToTotalScore(0);
        }

        var caches = FindObjectsOfType<HideCache>();

        foreach(var cache in caches)
        {
            if (cache.Owner != null
                && !cache.IsBroken)
            {
                cache.Owner.Player.PlayerData.TotalStolenLootRound += cache.HiddenGoldCount;
            }
        }
    }

    public void SetHiddenCacheOwner(InteractActor player)
    {
        var caches = FindObjectsOfType<HideCache>()
            .Where(cache => cache.Owner == null)
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
        PhotonView.RPC(nameof(RpcSync), RpcTarget.AllBufferedViaServer, GoldCount, IsGameOver, IsGoldCollected);
    }

    [PunRPC]
    private void RpcSync(int goldCount, bool isGameOver, bool isGoldCollected)
    {
        GoldCount = goldCount;
        IsGameOver = isGameOver;
        IsGoldCollected = isGoldCollected;
    }
}
