using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    public SpriteRenderer Crown;
    public PhotonView PhotonView { get; private set; }
    public ScoreSheetController ScoreSheetController { get; private set; }

    void Start()
    {
        PhotonView = GetComponent<PhotonView>();
        ScoreSheetController = FindObjectOfType<ScoreSheetController>();

        Crown.gameObject.SetActive(IsSingleTopPlayer());
    }

    void Update()
    {
        
    }

    private bool IsSingleTopPlayer()
    {
        if (ScoreSheetController.ActorScoreSheets.Count <= 1)
        {
            return ScoreSheetController.ActorScoreSheets.FirstOrDefault().Value.TotalScore > 0;
        }

        var highScorePlayers = ScoreSheetController.GetHighScorePlayers();

        if (highScorePlayers.Count == 1)
        {
            var topPlayer = highScorePlayers.First();

            return topPlayer == PhotonView.Owner;
        }

        return false;
    }
}
