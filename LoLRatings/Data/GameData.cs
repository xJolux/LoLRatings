using Newtonsoft.Json.Linq;
using System.Collections.Generic;


namespace LoLRatings.Data
{
    public class GameData
    {
        public List<JToken> PlayerJsonDataList { get; }
        public List<JToken> EventJsonDataList { get; }
        public string GameMode { get; }
        public bool IsLive { get; }

        public GameData(JObject allLiveJsonData)
        {
            PlayerJsonDataList = allLiveJsonData?["allPlayers"]?.ToObject<List<JToken>>() ?? new List<JToken>();
            EventJsonDataList = allLiveJsonData?["events"]?["Events"]?.ToObject<List<JToken>>() ?? new List<JToken>();
            GameMode = allLiveJsonData?["gameData"]?["gameMode"]?.ToString();
            IsLive = !allLiveJsonData?["activePlayer"]?.ToObject<JObject>().ContainsKey("error") ?? false;
        }

        public bool IsAvailable()
        {
            return PlayerJsonDataList.Count > 0 &&
                   EventJsonDataList.Count > 0 &&
                   !string.IsNullOrEmpty(GameMode);
        }
    }
}
