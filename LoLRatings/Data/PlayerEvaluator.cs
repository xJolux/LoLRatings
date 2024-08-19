using Newtonsoft.Json.Linq;
using System.Collections.Generic;

using LoLRatings.Data;
using LoLRatings.Data.EventHandlers;
using System;


namespace LoLRatings
{
    public static class PlayerEvaluator
    {
        private static readonly Dictionary<string, Func<EventData, PlayerRepository, bool>> _eventHandlers = new Dictionary<string, Func<EventData, PlayerRepository, bool>>
        {
            // Kill Events
            { "ChampionKill", (eventData, playerRepository) => ChampionKill.Handle(eventData, playerRepository) },
            { "FirstBlood", (eventData, playerRepository) => FirstBlood.Handle(eventData) },

            // Building Events
            { "TurretKilled", (eventData, playerRepository) => TurretKilled.Handle(eventData, playerRepository) },
            { "FirstBrick", (eventData, playerRepository) => FirstBrick.Handle(eventData, playerRepository) },
            { "InhibKilled", (eventData, playerRepository) => InhibKilled.Handle(eventData, playerRepository) },

            // Objective Events
            { "DragonKill", (eventData, playerRepository) => DragonKill.Handle(eventData) },
            { "HordeKill", (eventData, playerRepository) => HordeKill.Handle(eventData) },
            { "HeraldKill", (eventData, playerRepository) => HeraldKill.Handle(eventData) },
            { "BaronKill", (eventData, playerRepository) => BaronKill.Handle(eventData) }
        };

        public static bool UpdatePlayerListPassive(PlayerRepository playerRepository, List<JToken> playerJsonDataList)
        {
            foreach (JToken playerData in playerJsonDataList)
            {
                if (playerData == null)
                {
                    return false;
                }

                // Skip dummy
                if (playerData["rawChampionName"]?.ToString() == "game_character_displayname_PracticeTool_TargetDummy")
                {
                    continue;
                }

                // Get player
                Player player = playerRepository.GetPlayer(playerData["riotIdGameName"]?.ToString());

                if (player == null)
                {
                    return false;
                }

                // Get scores
                JToken scores = playerData["scores"];

                if (scores == null)
                {
                    return false;
                }
                
                // Get creep score and ward score
                JToken creepScoreData = scores["creepScore"];
                JToken wardScoreData = scores["wardScore"];

                if (creepScoreData == null || wardScoreData == null)
                {
                    return false;
                }

                int creepScore = creepScoreData.ToObject<int>();
                double wardScore = wardScoreData.ToObject<double>();

                // Calculate rating gain
                int rating = 0;
                rating += creepScore * Passive.CREEP_SCORE;
                rating += (int)(wardScore * Passive.WARD_SCORE);

                // Update player rating
                player.Rating += rating;
            }

            return true;
        }

        public static bool UpdatePlayerListEvents(PlayerRepository playerRepository, List<JToken> eventJsonDataList)
        {
            if (playerRepository.PlayerList.Count == 0)
            {
                return false;
            }

            foreach (JToken eventJsonData in eventJsonDataList)
            {
                if (eventJsonData == null)
                {
                    return false;
                }

                // Get event
                EventData eventData = new EventData(playerRepository, eventJsonData);

                // Handle event
                if (_eventHandlers.TryGetValue(eventData.Name, out Func<EventData, PlayerRepository, bool> handler))
                {
                    if (!handler(eventData, playerRepository))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
