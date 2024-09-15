using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class EndGameScorePanel : MonoBehaviour
{
    public EndGamePlayerScorePanel EndGamePlayerScorePanelTemplate;
    public TMP_Text EndGameInfoText;

    private UiStackHandler<EndGamePlayerScorePanel> _endGamePlayerScorePanelStack;
    private ScoreSheetController _scoreSheetController;
    private RulesController _rulesController;

    void Start()
    {
        EndGamePlayerScorePanelTemplate.gameObject.SetActive(false);
        _endGamePlayerScorePanelStack = new UiStackHandler<EndGamePlayerScorePanel>(this, EndGamePlayerScorePanelTemplate, 5f);
        _scoreSheetController = FindObjectOfType<ScoreSheetController>();
        _rulesController = FindObjectOfType<RulesController>();

        Invoke(nameof(Redraw), 0.2f);
    }

    void Update()
    {
        EndGameInfoText.text = _rulesController.IsGoldCollected 
            ? "Good job! Quota is reached (Worst thief loses)."
            : "Bad dwarves! Quota is not reached (Best thief wins).";
    }

    private void Redraw()
    {
        var sheets = _scoreSheetController.ActorScoreSheets.Values
            .OrderByDescending(sheet => sheet.TotalStolenLootRound);

        _endGamePlayerScorePanelStack.Redraw(
               sheets,
               (EndGamePlayerScorePanel panel, ScoreSheetController.ActorScoreSheet sheet) =>
               {
                   panel.Sheet = sheet;
               }
           );

        Invoke(nameof(Redraw), 0.2f);
    }
}
