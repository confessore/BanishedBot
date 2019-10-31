using BanishedBot.Statics;
using Discord.WebSocket;
using System.Linq;
using System.Threading.Tasks;

namespace BanishedBot.Services
{
    internal class BaseService
    {
        readonly DiscordSocketClient client;

        public BaseService(DiscordSocketClient client)
        {
            this.client = client;
        }

        public string ParseRole(string reaction)
        {
            return (reaction.ToLower()) switch
            {
                "warriortank" => "warrior",
                "druidbear" => "druid",
                "shamanresto" => "shaman",
                "priestholy" => "priest",
                "druidresto" => "druid",
                "mage" => "mage",
                "warlock" => "warlock",
                "warriordps" => "warrior",
                "hunter" => "hunter",
                "rogue" => "rogue",
                "shamanelemental" => "shaman",
                "priestshadow" => "priest",
                "shamanenhancement" => "shaman",
                "druidboomkin" => "druid",
                "druidcat" => "druid",
                _ => string.Empty,
            };
        }

        public async Task ModifyRole(SocketGuild guild, SocketGuildUser user, string name)
        {
            var roles = client.GetGuild(guild.Id).GetUser(user.Id).Roles;
            foreach (var role in roles)
            {
                if (Strings.Roles.Contains(role.Name.ToLower()))
                    await client.GetGuild(guild.Id).GetUser(user.Id).RemoveRoleAsync(role);
            }
            await client.GetGuild(guild.Id).GetUser(user.Id).AddRoleAsync(GetGuildRole(guild, name));
        }

        public SocketRole GetGuildRole(SocketGuild guild, string name) =>
            client.GetGuild(guild.Id).Roles.FirstOrDefault(x => x.Name.ToLower() == name.ToLower());

        public ISocketMessageChannel RoleChannel =>
            Guild.TextChannels.FirstOrDefault(x => x.Name == Strings.RoleChannel);
        public ISocketMessageChannel RaidChannel =>
            Guild.TextChannels.FirstOrDefault(x => x.Name == Strings.RaidChannel);
        public ISocketMessageChannel OutputChannel =>
            Guild.TextChannels.FirstOrDefault(x => x.Name == Strings.OutputChannel);
        public SocketGuild Guild =>
            client.Guilds.FirstOrDefault(x => x.Name == Strings.GuildName);
    }
}
