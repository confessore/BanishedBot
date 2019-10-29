using Discord;

namespace BanishedBot.Models
{
    public class Raider
    {
        public IUser User { get; set; }
        public string Name { get; set; }
        public IEmote Role { get; set; }
    }
}
