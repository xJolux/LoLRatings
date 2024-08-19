using System.Collections.Generic;


namespace LoLRatings.Data.EventHandlers
{
    public static class EventHelper
    {
        public static bool UpdateAssistersRating(List<Player> assisters, int value)
        {
            foreach (Player assister in assisters)
            {
                if (assister == null)
                {
                    return false;
                }

                assister.Rating += value;
            }

            return true;
        }

        public static bool UpdateKillerAssistersRating(Player killer, List<Player> assisters, int eachValue)
        {
            if (killer != null)
            {
                killer.Rating += eachValue;
            }

            if (assisters.Count > 0)
            {
                if (!UpdateAssistersRating(assisters, eachValue))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool UpdateTeamRating(PlayerRepository playerRepository, string entityName, int value)
        {
            if (entityName.Contains("T1"))
            {
                playerRepository.TeamRatings[Game.ORDER] += value;
            }
            else if (entityName.Contains("T2"))
            {
                playerRepository.TeamRatings[Game.CHAOS] += value;
            }
            else
            {
                return false;
            }

            return true;
        }
    }
}
