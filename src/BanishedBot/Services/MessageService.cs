using BanishedBot.Statics;
using Discord;
using Discord.WebSocket;
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

        public async Task CheckMessages()
        {
            var tmp = baseService.Channel.GetMessagesAsync().Flatten();
            if (await tmp.Count() > 0)
                await tmp.ForEachAsync(async x =>
                {
                    if (x.Author.Id != client.CurrentUser.Id)
                        await x.DeleteAsync();
                });
            if (await tmp.Count() == 0)
            {
                var msg = await baseService.Channel.SendMessageAsync("select the role that you intend to raid with.\nyou may only choose one role.");
                foreach (var reaction in Strings.Reactions)
                    await msg.AddReactionAsync(baseService.Guild.Emotes.FirstOrDefault(x => x.Name.ToLower() == reaction.ToLower()));
            }
        }
    }
}
