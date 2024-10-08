using Photon.Pun;
using TMPro;
using UnityEngine;

public class EndGamePlayerScorePanel : MonoBehaviour
{
    public ScoreSheetController.ActorScoreSheet Sheet;
    public TMP_Text NameText;
    public TMP_Text StolenLootText;
    public TMP_Text CollectedGoldText;
    public TMP_Text TotalScoreText;

    void Update()
    {
        if (Sheet != null)
        {
            NameText.text = PhotonNetwork.CurrentRoom.Players[Sheet.ActorNumber].NickName;

            StolenLootText.text = Sheet.TotalStolenLootRound.ToString();
            CollectedGoldText.text = Sheet.TotalGoldCollectedRound.ToString();


            TotalScoreText.text = Sheet.AddToTotalScoreRound == 0
                ? $"{Sheet.TotalScore}"
                : $"(+{Sheet.AddToTotalScoreRound}) {Sheet.TotalScore}";
        }
    }
}
