using Assets.Scripts.GameRulesModule.ScoreHandlers.Interfaces;
using System.Linq;

namespace Assets.Scripts.GameRulesModule.ScoreHandlers
{
    public class TopThiefScoreHandler : IScoreHandler
    {
        public void SetScores(ScoreSheetController scoreSheetController)
        {
            var ruleController = UnityEngine.Object.FindObjectOfType<RulesController>();

            var addScore = ruleController.IsGoldCollected
                ? 1
                : 3;

            if (!scoreSheetController.ActorScoreSheets.Values.All(sheet => sheet.TotalStolenLootRound == 0))
            {
                var sortedGroup = scoreSheetController.ActorScoreSheets.Values
                    .Where(player => player.TotalStolenLootRound > 0)
                    .GroupBy(player => player.TotalStolenLootRound)
                    .OrderByDescending(group => group.Key);

                var topPlayersGroup = sortedGroup.FirstOrDefault().ToList();

                foreach (var player in topPlayersGroup)
                {
                    player.AddToTotalScoreRound += addScore;
                }
            }
        }
    }
}
