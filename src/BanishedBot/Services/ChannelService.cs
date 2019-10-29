using BanishedBot.Statics;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BanishedBot.Services
{
    internal class ChannelService
    {
        readonly DiscordSocketClient client;

        public ChannelService(DiscordSocketClient client)
        {
            this.client = client;
        }

        public async Task CheckChannelsAsync()
        {
            Console.WriteLine(Guild.Name);
            if (!Guild.TextChannels.Any(x => x.Name == Strings.RoleChannel))
                await Guild.CreateTextChannelAsync(Strings.RoleChannel);
        }

        SocketGuild Guild =>
            client.Guilds.FirstOrDefault(x => x.Name == Strings.GuildName);
    }
}
