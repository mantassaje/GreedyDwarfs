using Assets.Scripts.GameRulesModule.ScoreHandlers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.GameRulesModule.ScoreHandlers.Special
{
    public class ResetScoreHandler : IScoreHandler
    {
        public void SetScores(ScoreSheetController scoreSheetController)
        {
            foreach (var sheet in scoreSheetController.ActorScoreSheets.Values)
            {
                sheet.TotalGoldCollectedRound = 0;
                sheet.TotalStolenLootRound = 0;
                sheet.AddToTotalScoreRound = 0;
            }
        }
    }
}
