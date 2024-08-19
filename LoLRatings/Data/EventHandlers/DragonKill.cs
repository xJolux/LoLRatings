using Newtonsoft.Json.Linq;


namespace LoLRatings.Data.EventHandlers
{
    internal class DragonKill
    {
        public static bool Handle(EventData eventData)
        {
            // Check if dragon type is available
            if (eventData.DragonType == null)
            {
                return false;
            }

            // Get value
            int killValue = eventData.DragonType == "Elder" ? Objective.ELDER : Objective.DRAGON;

            return ObjectiveEventHandler.HandleObjectiveKill(eventData, killValue);
        }
    }
}
