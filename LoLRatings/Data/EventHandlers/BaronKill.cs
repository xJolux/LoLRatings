namespace LoLRatings.Data.EventHandlers
{
    internal class BaronKill
    {
        public static bool Handle(EventData eventData)
        {
            return ObjectiveEventHandler.HandleObjectiveKill(eventData, Objective.BARON);
        }
    }
}
