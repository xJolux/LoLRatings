using Newtonsoft.Json.Linq;


namespace LoLRatings.Data.EventHandlers
{
    public static class FirstBrick
    {
        public static bool Handle(EventData eventData, PlayerRepository playerRepository)
        {
            // Check if killer is available
            if (eventData.Killer == null)
            {
                // Give rating to team
                return EventHelper.UpdateTeamRating(playerRepository, eventData.EntityName, Building.FIRST_BRICK);
            }

            // Update killer
            eventData.Killer.Rating += Building.FIRST_BRICK;

            return true;
        }
    }
}
