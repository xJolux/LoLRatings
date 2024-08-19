using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;


namespace LoLRatings.Data
{
    public class EventData
    {
        public string Name { get; }
        public double Time { get; }

        public Player Killer { get; }
        public string EntityName { get; }
        public Player Recipient { get; }
        public Player Victim { get; }
        public List<Player> Assisters { get; }
        public string TurretName { get; }
        public string DragonType { get; }

        public EventData(PlayerRepository playerRepository, JToken eventData)
        {
            Name = eventData["EventName"].ToString();
            Time = (double)eventData["EventTime"];
            Killer = playerRepository.GetPlayer(eventData["KillerName"]?.ToString());
            EntityName = eventData["KillerName"]?.ToString();
            Recipient = playerRepository.GetPlayer(eventData["Recipient"]?.ToString());
            Victim = playerRepository.GetPlayer(eventData["VictimName"]?.ToString());
            Assisters = eventData["Assisters"]?.ToObject<List<string>>().Select(assister => playerRepository.GetPlayer(assister)).ToList();
            TurretName = eventData["TurretKilled"]?.ToString();
            DragonType = eventData["DragonType"]?.ToString();
        }
    }
}
