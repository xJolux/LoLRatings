using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;


namespace LoLRatings.Data.EventHandlers
{
    public static class TurretKilled
    {
        public static bool Handle(EventData eventData, PlayerRepository playerRepository)
        {
            // Check if data is available
            if (eventData.TurretName == null)
            {
                return false;
            }

            // Get data
            string killerTeam = eventData.TurretName.Contains("T2") ? Game.ORDER : Game.CHAOS;
            int teamSize = playerRepository.GetTeam(killerTeam).Count;

            // Calculate turret value
            int turretValue = CalculateTurretValue(teamSize, eventData.TurretName, eventData.Time);

            // Give rating to team
            if (eventData.Killer == null && eventData.Assisters.Count == 0)
            {
                return EventHelper.UpdateTeamRating(playerRepository, eventData.EntityName, turretValue);
            }

            // Get each value
            int eachValue = turretValue / (eventData.Assisters.Count + (eventData.Killer != null ? 1 : 0));

            // Update killer and assisters
            return EventHelper.UpdateKillerAssistersRating(eventData.Killer, eventData.Assisters, eachValue);
        }

        // Calculate the turret value based on its position and time
        private static int CalculateTurretValue(int teamSize, string turretName, double time)
        {
            int localValue = 0;
            int globalValue = 0;

            // outer turret
            if (turretName.Contains("C_05") || (!turretName.Contains("C") && turretName.Contains("03")))
            {
                // value based on time
                localValue = Building.OUTER_LOCAL + (time <= Building.PLATING_TIME ? Building.TOTAL_PLATING : Building.TOTAL_PLATING / 2);
                globalValue = Building.OUTER_GLOBAL * teamSize;
            }
            // inner turret
            else if (turretName.Contains("C_04") || (!turretName.Contains("C") && turretName.Contains("02")))
            {
                // value based on position
                localValue = turretName.Contains("C") ? Building.INNER_CENTER_LOCAL : Building.INNER_SIDE_LOCAL;
                globalValue = Building.INNER_GLOBAL * teamSize;
            }
            // inhib turret
            else if (turretName.Contains("C_03") || (!turretName.Contains("C") && turretName.Contains("01")))
            {
                localValue = Building.INHIB_LOCAL;
                globalValue = Building.INHIB_GLOBAL * teamSize;
            }
            // nexus turret
            else
            {
                localValue = Building.NEXUS_LOCAL;
                globalValue = Building.NEXUS_GLOBAL * teamSize;
            }

            return localValue + globalValue;
        }
    }
}
