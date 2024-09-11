using Photon.Pun;
using UnityEngine;

/// <summary>
/// Player data object outside moving player character.
/// Is invisible data object.
/// </summary>
public class PlayerData : MonoBehaviour
{
    public PhotonView PhotonView { get; private set; }
    public string Name { get; private set; }
    public int TotalScore { get; private set; }

    public int AddToTotalScoreRound { get; private set; }
    public int TotalStolenLootRound;

    private void Awake()
    {
        PhotonView = GetComponent<PhotonView>();
        Name = "Test Name";
    }

    private void Start()
    {
        //DontDestroyOnLoad(gameObject);
    }

    public void SetName(string name)
    {
        Name = name;
        PhotonView.RPC(nameof(RpcSetName), RpcTarget.AllBufferedViaServer, Name);
    }

    [PunRPC]
    private void RpcSetName(string name)
    {
        Name = name;
    }

    public void AddToTotalScore(int addScore)
    {
        AddToTotalScoreRound = addScore;
        TotalScore += addScore;
        PhotonView.RPC(nameof(Sync), RpcTarget.AllBufferedViaServer, TotalScore, AddToTotalScoreRound, TotalStolenLootRound);
    }

    [PunRPC]
    private void Sync(int totalScore, int addToTotalScoreRound, int totalStolenLootRound)
    {
        TotalScore = totalScore;
        AddToTotalScoreRound = addToTotalScoreRound;
        TotalStolenLootRound = totalStolenLootRound;
    }


}
