using BanishedBot.Statics;
using Discord.WebSocket;
using System.Linq;
using System.Threading.Tasks;

namespace BanishedBot.Services
{
    internal class ChannelService
    {
        readonly DiscordSocketClient client;
        readonly BaseService baseService;

        public ChannelService(
            DiscordSocketClient client,
            BaseService baseService)
        {
            this.client = client;
            this.baseService = baseService;
        }

        public async Task CheckChannelsAsync()
        {
            if (!baseService.Guild.TextChannels.Any(x => x.Name == Strings.RoleChannel))
                await baseService.Guild.CreateTextChannelAsync(Strings.RoleChannel);
        }
    }
}
