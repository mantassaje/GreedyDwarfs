using Assets.Scripts.GameRulesModule.ScoreHandlers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.GameRulesModule.ScoreHandlers
{
    public class GoldCollectedAllScoreHandler : IScoreHandler
    {
        public void SetScores(ScoreSheetController scoreSheetController)
        {
            var ruleController = UnityEngine.Object.FindObjectOfType<RulesController>();

            if (!ruleController.IsGoldCollected)
            {
                return;
            }

            var minCollected = scoreSheetController.ActorScoreSheets.Values.Select(sheet => sheet.TotalGoldCollectedRound).Min();

            if (scoreSheetController.ActorScoreSheets.Values.All(sheet => sheet.TotalGoldCollectedRound == minCollected))
            {
                minCollected = 0;
            }

            foreach (var sheet in scoreSheetController.ActorScoreSheets.Values)
            {
                if (sheet.TotalGoldCollectedRound > minCollected)
                {
                    sheet.AddToTotalScoreRound += 1;
                }
            }
        }
    }
}
