using Assets.Scripts.GameRulesModule.ScoreHandlers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.GameRulesModule.ScoreHandlers.Special
{
    public class CollectCountsScoreHandler : IScoreHandler
    {

        public void SetScores(ScoreSheetController scoreSheetController)
        {
            SetStolenLoot(scoreSheetController);
            SetCollectedGold(scoreSheetController);
        }

        private void SetStolenLoot(ScoreSheetController scoreSheetController)
        {
            var caches = UnityEngine.Object.FindObjectsOfType<HideCache>();

            foreach (var cache in caches)
            {
                if (cache.Owner != null
                    && !cache.IsBroken)
                {
                    var stolenGold = cache.HiddenGoldCount;

                    // In case player left and some object are now null by mistake.
                    if (cache.Owner.Player?.PhotonView?.Owner == null)
                    {
                        continue;
                    }

                    scoreSheetController.ActorScoreSheets[cache.Owner.Player.PhotonView.Owner.ActorNumber].TotalStolenLootRound += stolenGold;
                }
            }
        }

        private void SetCollectedGold(ScoreSheetController scoreSheetController)
        {
            var playerTokens = UnityEngine.Object.FindObjectsOfType<PlayerToken>()
                .Where(token => token?.PhotonView?.Owner != null)
                .ToList();

            foreach (var token in playerTokens)
            {
                scoreSheetController.ActorScoreSheets[token.PhotonView.Owner.ActorNumber].TotalGoldCollectedRound = token.TotalGoldCollected;
            }
        }
    }
}
