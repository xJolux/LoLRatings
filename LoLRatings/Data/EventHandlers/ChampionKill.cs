using System;


namespace LoLRatings.Data.EventHandlers
{
    public static class ChampionKill
    {
        public static bool Handle(EventData eventData, PlayerRepository playerRepository)
        {
            // If the killer is not available but the victim is, update the team ratings
            if (eventData.Killer == null && eventData.Victim != null)
            {
                playerRepository.TeamRatings[eventData.Victim.Team == Game.ORDER ? Game.CHAOS : Game.ORDER] += Kill.KILL;

                return true;
            }

            // Check if both players are available
            if (eventData.Killer == null || eventData.Victim == null)
            {
                return false;
            }

            // Calculate kill and assist values
            int killValue = CalculateKillValue(eventData.Victim);
            int assistValue = CalculateAssistValue(eventData.Victim);

            // Update killer
            UpdateKiller(eventData.Killer, killValue);

            // Update victim
            UpdateVictim(eventData.Victim);

            // Update assisters
            if (eventData.Assisters.Count > 0)
            {
                int eachAssistValue = assistValue / eventData.Assisters.Count;
                if (!EventHelper.UpdateAssistersRating(eventData.Assisters, eachAssistValue))
                {
                    return false;
                }
            }

            return true;
        }

        // Calculate the kill value based on the victims bounty
        private static int CalculateKillValue(Player victim)
        {
            int killValue = Kill.KILL;
            int bountyValue = 0;

            if (victim.Bounty > 1)
            {
                bountyValue = 100 * (victim.Bounty - 1);
                killValue += Math.Min(bountyValue, 700);
            }
            else if (victim.Bounty < 0)
            {
                bountyValue = victim.Bounty * 40;
                killValue += Math.Max(bountyValue, -200);
            }

            return killValue;
        }

        // Calculate the assist value based on the victims bounty
        private static int CalculateAssistValue(Player victim)
        {
            int assistValue = Kill.ASSIST;

            if (victim.Bounty < 0)
            {
                int bountyAssistValue = victim.Bounty * 20;
                assistValue += Math.Max(bountyAssistValue, -100);
            }

            return assistValue;
        }

        // Update the killers rating and bounty
        private static void UpdateKiller(Player killer, int killValue)
        {
            killer.Rating += killValue;
            if (killer.Bounty < 0)
            {
                killer.Bounty = 0;
            }
            killer.Bounty += 1;
        }

        // Update the victims bounty
        private static void UpdateVictim(Player victim)
        {
            if (victim.Bounty > 0)
            {
                victim.Bounty -= 8;
                if (victim.Bounty < 0)
                {
                    victim.Bounty = 0;
                }
            }
            else
            {
                victim.Bounty -= 1;
            }
        }
    }
}
