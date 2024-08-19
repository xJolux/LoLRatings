namespace LoLRatings.Data.EventHandlers
{
    internal class HordeKill
    {
        public static bool Handle(EventData eventData)
        {
            return ObjectiveEventHandler.HandleObjectiveKill(eventData, Objective.HORDE);
        }
    }
}
