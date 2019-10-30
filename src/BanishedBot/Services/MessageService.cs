using BanishedBot.Statics;
using Discord;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BanishedBot.Services
{
    internal class MessageService
    {
        readonly DiscordSocketClient client;
        readonly BaseService baseService;

        public MessageService(
            DiscordSocketClient client,
            BaseService baseService)
        {
            this.client = client;
            this.baseService = baseService;
        }

        public async Task CheckMessagesAsync()
        {
            await DeleteMessagesAsync(baseService.RoleChannel);
            await DeleteMessagesAsync(baseService.RaidChannel);
            await CheckRoleMessageAsync();
        }

        async Task CheckRoleMessageAsync()
        {
            var tmp = baseService.RoleChannel.GetMessagesAsync().Flatten();
            if (await tmp.Count() == 0)
            {
                var msg = await baseService.RoleChannel.SendMessageAsync("select the role that you intend to raid with.\nyou may only choose one role.");
                foreach (var reaction in Strings.Reactions)
                    await msg.AddReactionAsync(baseService.Guild.Emotes.FirstOrDefault(x => x.Name.ToLower() == reaction.ToLower()));
            }
        }

        async Task DeleteMessagesAsync(ISocketMessageChannel channel)
        {
            var tmp = channel.GetMessagesAsync().Flatten();
            await tmp.ForEachAsync(async x =>
            {
                var admin = baseService.Guild.Users.FirstOrDefault(y => y.Id == x.Author.Id).GuildPermissions.Administrator;
                if (!admin)
                    await x.DeleteAsync();
            });
        }
    }
}
