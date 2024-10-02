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

            foreach (var sheet in scoreSheetController.ActorScoreSheets.Values)
            {
                if (sheet.TotalGoldCollectedRound > 0)
                {
                    sheet.AddToTotalScoreRound += 1;
                }
            }
        }
    }
}
