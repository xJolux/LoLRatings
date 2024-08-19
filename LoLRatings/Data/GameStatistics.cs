using System;
using System.Collections.Generic;

using LoLRatings.Data.Prediction;


namespace LoLRatings.Data
{
    public class GameStatistics
    {
        private PlayerRepository PlayerRepository;

        public GameStatistics(PlayerRepository playerRepository)
        {
            PlayerRepository = playerRepository;
        }

        public Dictionary<string, float> GetRatingPercentages()
        {
            float totalOrderRating = PlayerRepository?.GetTeamTotalRating(Game.ORDER) ?? 0;
            float totalChaosRating = PlayerRepository?.GetTeamTotalRating(Game.CHAOS) ?? 0;
            float totalRating = totalOrderRating + totalChaosRating;

            float orderWinPercent = 0;
            float chaosWinPercent = 0;
            if (totalRating > 0)
            {
                orderWinPercent = totalOrderRating / totalRating * 100;
                chaosWinPercent = totalChaosRating / totalRating * 100;
            }

            return new Dictionary<string, float>
            {
                { Game.ORDER, orderWinPercent },
                { Game.CHAOS, chaosWinPercent }
            };
        }

        public Dictionary<string, float> GetWinPercentages()
        {
            var teamsRatingPercent = GetRatingPercentages();
            float orderRatingPercent = teamsRatingPercent[Game.ORDER];
            float chaosRatingPercent = teamsRatingPercent[Game.CHAOS];
            float teamsRatingPercentDiff = chaosRatingPercent - orderRatingPercent;

            float orderWinPercent = (float)(1 / (1 + Math.Pow(10, (teamsRatingPercentDiff / Game.SENSITIVITY))));
            float chaosWinPercent = (1 - orderWinPercent);

            return new Dictionary<string, float>
            {
                { Game.ORDER, orderWinPercent * 100 },
                { Game.CHAOS, chaosWinPercent * 100 }
            };
        }

        public float GetRatingPercentDiff()
        {
            var teamsRatingPercent = GetRatingPercentages();
            float orderRatingPercent = teamsRatingPercent[Game.ORDER];
            float chaosRatingPercent = teamsRatingPercent[Game.CHAOS];
            float teamsRatingPercentDiff = Math.Abs(chaosRatingPercent - orderRatingPercent);

            return teamsRatingPercentDiff;
        }

        public float GetEndTime()
        {
            float ratingPercentDiff = GetRatingPercentDiff();
            float predictedEndTime = PredictionManager.PredictEndTime(ratingPercentDiff);
            
            return predictedEndTime;
        }
    }
}
