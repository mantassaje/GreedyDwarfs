using Assets.Scripts.GameRulesModule.ScoreHandlers;
using Assets.Scripts.GameRulesModule.ScoreHandlers.Interfaces;
using Assets.Scripts.GameRulesModule.ScoreHandlers.Special;
using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class RulesController : MonoBehaviour, IPunObservable
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

    private List<IScoreHandler> ScoreHandlers;

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
        }

        ScoreHandlers = new List<IScoreHandler>()
        {
            // Setup
            new ResetScoreHandler(),
            new CollectCountsScoreHandler(),

            // Score logic
            new GoldCollectedAllScoreHandler(),
            new TopThiefScoreHandler(),
            new TopWorkerScoreHandler(),

            // Commit
            new CommitTotalScoreHandler()
        };
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
        }
    }

    private void EndTime()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            SetGameOverEndTimeScores();
            SetEndGame();
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
            PhotonNetwork.DestroyAll();
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
            CalculateScore();
        }
    }

    public void SetGameOverGoalSuccessScores()
    {
        if (PhotonNetwork.IsMasterClient
            && !IsGameOver)
        {
            IsGoldCollected = true;
            CalculateScore();
        }
    }

    private void CalculateScore()
    {
        foreach(var handler in ScoreHandlers)
        {
            handler.SetScores(ScoreSheetController);
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(GoldGoal);
            stream.SendNext(GoldCount);
            stream.SendNext(IsGameOver);
            stream.SendNext(IsGoldCollected);
        }
        else
        {
            GoldGoal = (int)stream.ReceiveNext();
            GoldCount = (int)stream.ReceiveNext();
            IsGameOver = (bool)stream.ReceiveNext();
            IsGoldCollected = (bool)stream.ReceiveNext();
        }
    }
}
