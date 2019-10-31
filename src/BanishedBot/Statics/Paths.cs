using System.IO;

namespace BanishedBot.Statics
{
    internal static class Paths
    {
        public static string ZG(int mod) => Directory.GetCurrentDirectory() + Strings.ZG(mod);
        public static string AQR(int mod) => Directory.GetCurrentDirectory() + Strings.AQR(mod);
        public static string MCT(int mod) => Directory.GetCurrentDirectory() + Strings.MCT(mod);
        public static string MC(int mod) => Directory.GetCurrentDirectory() + Strings.MC(mod);
        public static string ONY(int mod) => Directory.GetCurrentDirectory() + Strings.ONY(mod);
        public static string BWL(int mod) => Directory.GetCurrentDirectory() + Strings.BWL(mod);
    }
}
