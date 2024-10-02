using Assets.Scripts.GameRulesModule.ScoreHandlers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.GameRulesModule.ScoreHandlers
{
    public class TopWorkerScoreHandler : IScoreHandler
    {
        public void SetScores(ScoreSheetController scoreSheetController)
        {
            var ruleController = UnityEngine.Object.FindObjectOfType<RulesController>();

            if (!ruleController.IsGoldCollected)
            {
                return;
            }

            if (!scoreSheetController.ActorScoreSheets.Values.All(sheet => sheet.TotalGoldCollectedRound == 0))
            {
                var sortedGroup = scoreSheetController.ActorScoreSheets.Values
                    .Where(player => player.TotalGoldCollectedRound > 0)
                    .GroupBy(player => player.TotalGoldCollectedRound)
                    .OrderByDescending(group => group.Key);

                var topPlayersGroup = sortedGroup.FirstOrDefault().ToList();

                foreach (var player in topPlayersGroup)
                {
                    player.AddToTotalScoreRound += 1;
                }
            }
        }
    }
}
