using BanishedBot.Statics;
using Discord;
using Discord.Rest;
using Discord.WebSocket;
using System.Linq;
using System.Threading.Tasks;

namespace BanishedBot.Services
{
    internal class MessageService
    {
        readonly DiscordSocketClient client;

        public MessageService(DiscordSocketClient client)
        {
            this.client = client;
        }

        public async Task CheckMessages()
        {
            var tmp = Channel.GetMessagesAsync().Flatten();
            if (await tmp.Count() > 0)
                await tmp.ForEachAsync(async x =>
                {
                    if (x.Author.Id != client.CurrentUser.Id)
                        await x.DeleteAsync();
                });
            if (await tmp.Count() == 0)
            {
                var msg = await Channel.SendMessageAsync("select the role that you intend to raid with");
                foreach (var reaction in Strings.Reactions)
                    await msg.AddReactionAsync(Guild.Emotes.FirstOrDefault(x => x.Name.ToLower() == reaction.ToLower()));
            }
        }

        ISocketMessageChannel Channel =>
            Guild.TextChannels.FirstOrDefault(x => x.Name == Strings.RoleChannel);

        SocketGuild Guild =>
            client.Guilds.FirstOrDefault(x => x.Name == Strings.GuildName);
    }
}
