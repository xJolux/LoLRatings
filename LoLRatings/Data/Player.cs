using Newtonsoft.Json.Linq;


namespace LoLRatings.Data
{
    public class Player
    {
        public string Name { get; set; }
        public string SummonerName { get; set; }
        public string Team { get; set; }
        public string Position { get; set; }
        public string DisplayName { get; set; }
        public int Bounty { get; set; }
        public int Rating { get; set; }
        public int Rank { get; set; }
        public int MinMax { get; set; }
        public int MaxPercent { get; set; }
        public int Performance { get; set; }

        public Player(JToken playerData)
        {
            Name = playerData["riotIdGameName"].ToString();
            SummonerName = playerData["summonerName"].ToString();
            Team = playerData["team"].ToString();
            Position = playerData["position"].ToString();
            DisplayName = $"({(Game.POSITION_SHORT.TryGetValue(Position, out string pos) ? pos : Position)}) {playerData["championName"]}";

            Bounty = 0;

            Rating = 0;
            Rank = 0;
            MinMax = 0;
            MaxPercent = 0;
            Performance = 0;
        }
    }
}