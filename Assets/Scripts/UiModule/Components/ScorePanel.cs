using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ScorePanel : MonoBehaviour
{
    public ScorePlayerPanel ScorePlayerPanelTemplate;

    private UiStackHandler<ScorePlayerPanel> _endGamePlayerScorePanelStack;
    private ScoreSheetController _scoreSheetController;

    void Start()
    {
        ScorePlayerPanelTemplate.gameObject.SetActive(false);
        _endGamePlayerScorePanelStack = new UiStackHandler<ScorePlayerPanel>(this, ScorePlayerPanelTemplate, 5f);
        _scoreSheetController = FindObjectOfType<ScoreSheetController>();

        Invoke(nameof(Redraw), 0.2f);
    }

    private void Redraw()
    {
        var sheets = _scoreSheetController.ActorScoreSheets.Values
            .OrderByDescending(sheet => sheet.TotalScore);

        _endGamePlayerScorePanelStack.Redraw(
               sheets,
               (ScorePlayerPanel panel, ScoreSheetController.ActorScoreSheet sheet) =>
               {
                   panel.Sheet = sheet;
               }
           );

        Invoke(nameof(Redraw), 0.2f);
    }
}
