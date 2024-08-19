namespace LoLRatings.Data.EventHandlers
{
    internal class HeraldKill
    {
        public static bool Handle(EventData eventData)
        {
            return ObjectiveEventHandler.HandleObjectiveKill(eventData, Objective.HERALD);
        }
    }
}
