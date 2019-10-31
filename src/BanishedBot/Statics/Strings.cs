namespace BanishedBot.Statics
{
    internal static class Strings
    {
        public static string GuildName => "Banished";
        public static string RoleChannel => $"🎯role-select";
        public static string RaidChannel => "📆raid-signups";
        public static string OutputChannel => "dev";
        public static string ZG(int mod) => $"//ZG{mod}";
        public static string AQR(int mod) => $"//AQR{mod}";
        public static string MCT(int mod) => $"//MCT{mod}";
        public static string MC(int mod) => $"//MC{mod}";
        public static string ONY(int mod) => $"//ONY{mod}";
        public static string BWL(int mod) => $"//BWL{mod}";

        public static string[] Roles =>
            new string[]
            {
                "druid",
                "mage",
                "shaman",
                "warrior",
                "warlock",
                "priest",
                "hunter",
                "rogue"
            };

        public static string[] Reactions =>
            new string[]
            {
                "warriortank",
                "druidbear",
                "shamanresto",
                "priestholy",
                "druidresto",
                "mage",
                "warlock",
                "warriordps",
                "hunter",
                "rogue",
                "shamanelemental",
                "priestshadow",
                "shamanenhancement",
                "druidboomkin",
                "druidcat"
            };
    }
}
