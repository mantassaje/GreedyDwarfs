using Photon.Pun;
using TMPro;
using UnityEngine;

public class ScorePlayerPanel : MonoBehaviour
{
    public ScoreSheetController.ActorScoreSheet Sheet;
    public TMP_Text NameText;
    public TMP_Text TotalScoreText;

    void Update()
    {
        if (Sheet != null)
        {
            NameText.text = PhotonNetwork.CurrentRoom.Players[Sheet.ActorNumber].NickName;

            TotalScoreText.text = $"{Sheet.TotalScore}";
        }
    }
}
