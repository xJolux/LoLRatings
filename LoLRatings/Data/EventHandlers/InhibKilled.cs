using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;


namespace LoLRatings.Data.EventHandlers
{
    internal class InhibKilled
    {
        public static bool Handle(EventData eventData, PlayerRepository playerRepository)
        {
            // Check if a player is available
            if (eventData.Killer == null && eventData.Assisters.Count == 0)
            {
                // Update team
                return EventHelper.UpdateTeamRating(playerRepository, eventData.EntityName, Building.INHIB);
            }

            // Get value
            int eachValue = Building.INHIB / (eventData.Assisters.Count + (eventData.Killer != null ? 1 : 0));

            // Update killer and assisters
            return EventHelper.UpdateKillerAssistersRating(eventData.Killer, eventData.Assisters, eachValue);
        }
    }
}
