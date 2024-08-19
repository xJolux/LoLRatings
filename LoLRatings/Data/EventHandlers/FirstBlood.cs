namespace LoLRatings.Data.EventHandlers
{
    public static class FirstBlood
    {
        public static bool Handle(EventData eventData)
        {
            // Check if recipient is available
            if (eventData.Recipient == null)
            {
                return false;
            }

            // Update recipient
            eventData.Recipient.Rating += Kill.FIRST_BLOOD;

            return true;
        }
    }
}
