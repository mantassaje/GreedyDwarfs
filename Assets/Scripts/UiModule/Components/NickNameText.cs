using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class NickNameText : MonoBehaviour
{
    public TMP_Text Text { get; private set; }
    public ScoreSheetController ScoreSheetController { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        Text = GetComponent<TMP_Text>();
        ScoreSheetController = FindObjectOfType<ScoreSheetController>();
    }

    // Update is called once per frame
    void Update()
    {
        ScoreSheetController.ActorScoreSheets.TryGetValue(PhotonNetwork.LocalPlayer.ActorNumber, out var scoreSheet);

        Text.text = $"{PhotonNetwork.LocalPlayer.NickName}            Steal {scoreSheet?.TotalStolenLootRound}              {scoreSheet?.TotalScore} (+{scoreSheet?.AddToTotalScoreRound})";
    }
}
