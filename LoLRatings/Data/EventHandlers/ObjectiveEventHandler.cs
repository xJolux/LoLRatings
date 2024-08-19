using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;


namespace LoLRatings.Data.EventHandlers
{
    public class ObjectiveEventHandler
    {
        public static bool HandleObjectiveKill(EventData eventData, int killValue)
        {
            // Get ally assisters
            List<Player> allyAssisters = eventData.Assisters.Where(currentPlayer => currentPlayer.Team == eventData.Killer.Team).ToList();

            // Check if a player is available
            if (eventData.Killer == null && allyAssisters.Count == 0)
            {
                return false;
            }

            // Get value
            int eachValue = killValue / (allyAssisters.Count + (eventData.Killer != null ? 1 : 0));

            // Update killer and assisters
            return EventHelper.UpdateKillerAssistersRating(eventData.Killer, allyAssisters, eachValue);
        }
    }
}
