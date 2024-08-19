using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;


namespace LoLRatings
{
    public class Constants
    {
        // Variables
        public const int UPDATE_INTERVAL = 3000;

        // Path
        public static readonly string TRANINGS_DATA_JSON_PATH = "Data\\TrainingsData.json";
        public static readonly string LOG_FILE_PATH = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "error.log");
    }

    public class Game
    {
        // Teams
        public const string ORDER = "ORDER";
        public const string CHAOS = "CHAOS";

        // Game Modes
        public static readonly string[] AVAILABLE_GAME_MODES = new string[]
        {
            "CLASSIC",
            "ARAM",
            "PRACTICETOOL"
        };

        // Positions
        public static readonly Dictionary<string, string> POSITION_SHORT = new Dictionary<string, string>()
        {
            { "TOP", "Top" },
            { "JUNGLE", "Jgl" },
            { "MIDDLE", "Mid" },
            { "BOTTOM", "Bot" },
            { "UTILITY", "Sup" },
            { "", "None" }
        };

        // Win percent sensitivity
        public const int SENSITIVITY = 30;
    }

    public class Passive
    {
        // Passive
        public const int CREEP_SCORE = 23;
        public const int WARD_SCORE = 60;
    }

    public class Kill
    {
        // ChampionKill
        public const int KILL = 300;
        public const int ASSIST = 150;

        // FirstBlood
        public const int FIRST_BLOOD = 100;
    }

    public class Building
    {
        // TurretKilled
        // Outer turret
        public const int OUTER_LOCAL = 250;
        public const int OUTER_GLOBAL = 50;

        // Inner turret
        public const int INNER_SIDE_LOCAL = 675;
        public const int INNER_CENTER_LOCAL = 425;
        public const int INNER_GLOBAL = 25;

        // Inhib turret
        public const int INHIB_LOCAL = 375;
        public const int INHIB_GLOBAL = 25;

        // Nexus turret
        public const int NEXUS_LOCAL = 0;
        public const int NEXUS_GLOBAL = 50;

        // Plating
        public const int TOTAL_PLATING = 625;
        public const int PLATING_TIME = 840;

        // FirstBrick
        public const int FIRST_BRICK = 300;

        // InhibKilled
        public const int INHIB = 1500;
    }

    public class Objective
    {
        // DragonKill
        public const int DRAGON = 1500; // 2000
        public const int ELDER = 3200; // 3000

        // HordeKill
        public const int HORDE = 500;

        // HeraldKill
        public const int HERALD = 1500;

        // BaronKill
        public const int BARON = 3000;
    }
}
