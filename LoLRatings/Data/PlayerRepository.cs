using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;


namespace LoLRatings.Data
{
    public class PlayerRepository
    {
        public List<Player> PlayerList { get; }

        public Dictionary<string, int> TeamRatings { get; set; }

        public PlayerRepository(List<JToken> playerJsonDataList)
        {
            PlayerList = InitPlayerList(playerJsonDataList);

            TeamRatings = new Dictionary<string, int>
            {
                { Game.ORDER, 0 },
                { Game.CHAOS, 0 }
            };
        }

        private List<Player> InitPlayerList(List<JToken> playerJsonDataList)
        {
            if (playerJsonDataList == null)
            {
                return new List<Player>();
            }

            List<Player> playerList = new List<Player>();

            foreach (JToken playerData in playerJsonDataList)
            {
                try
                {
                    Player player = new Player(playerData);
                    playerList.Add(player);
                }
                catch 
                {
                    return new List<Player>();
                }
            }

            return playerList;
        }

        public Player GetPlayer(string name)
        {
            Player player = PlayerList.FirstOrDefault(currentPlayer => currentPlayer.Name == name);
            if (player == null)
            {
                player = PlayerList.FirstOrDefault(currentPlayer => currentPlayer.SummonerName == name);
            }
            return player;
        }

        public List<Player> GetTeam(string team)
        {
            return PlayerList.Where(player => player.Team == team).ToList();
        }

        public int GetTeamTotalRating(string team)
        {
            int totalRating = 0;
            if (TeamRatings.ContainsKey(team))
            {
                totalRating = GetTeam(team).Sum(player => player.Rating) + TeamRatings[team];
            }

            return totalRating;
        }

        public void UpdatePlayerStatistics()
        {
            if (PlayerList.Count == 0)
            {
                return;
            }

            int totalRating = GetTeamTotalRating(Game.ORDER) + GetTeamTotalRating(Game.CHAOS);
            int averageRating = totalRating / PlayerList.Count;
            int highestRating = PlayerList.Max(player => player.Rating);
            int lowestRating = PlayerList.Min(player => player.Rating);
            var rankings = PlayerList
                .Select(player => player.Rating)
                .Distinct()
                .OrderByDescending(rating => rating)
                .Select((rating, index) => new { rating, rank = index + 1 })
                .ToDictionary(x => x.rating, x => x.rank);

            if (totalRating > 0)
            {
                foreach (Player player in PlayerList)
                {
                    player.Rank = rankings[player.Rating];

                    player.MinMax = (player.Rating - lowestRating) * 100 / (highestRating - lowestRating);

                    player.MaxPercent = player.Rating * 100 / highestRating;

                    player.Performance = player.Rating * 100 / averageRating;
                }
            }
        }
    }
}
