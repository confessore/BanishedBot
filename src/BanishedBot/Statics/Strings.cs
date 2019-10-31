namespace BanishedBot.Statics
{
    internal static class Strings
    {
        public static string GuildName => "Banished";
        public static string RoleChannel => $"🎯role-select";
        public static string RaidChannel => "📆raid-signups";
        public static string OutputChannel => "dev";
        public static string ZG => "//ZG.jpg";
        public static string AQR => "//AQR.jpg";
        public static string MC => "//MC.jpg";
        public static string MCT => "//MCT.jpg";

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
