using System.IO;

namespace BanishedBot.Statics
{
    internal static class Paths
    {
        public static string PathBuilder(string resourceName) => $"{Directory.GetCurrentDirectory()}//{resourceName.Replace('_', '.')}";
    }
}
