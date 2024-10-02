using Assets.Scripts.GameRulesModule.ScoreHandlers.Interfaces;

namespace Assets.Scripts.GameRulesModule.ScoreHandlers.Special
{
    public class CommitTotalScoreHandler : IScoreHandler
    {
        public void SetScores(ScoreSheetController scoreSheetController)
        {
            foreach (var sheet in scoreSheetController.ActorScoreSheets.Values)
            {
                sheet.TotalScore += sheet.AddToTotalScoreRound;
            }
        }
    }
}
