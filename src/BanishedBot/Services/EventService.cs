using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace BanishedBot.Discord.Services
{
    internal class EventService
    {
        readonly IServiceProvider services;
        readonly DiscordSocketClient client;
        readonly CommandService commandService;

        public EventService(
            IServiceProvider services,
            DiscordSocketClient client,
            CommandService commandService)
        {
            this.services = services;
            this.client = client;
            this.commandService = commandService;
            client.Disconnected += Disconnected;
            client.MessageReceived += MessageReceived;
            client.Ready += Ready;
        }

        Task Disconnected(Exception e)
        {
            Console.WriteLine(e);
            Environment.Exit(-1);
            return Task.CompletedTask;
        }

        async Task MessageReceived(SocketMessage msg)
        {
            var tmp = (SocketUserMessage)msg;
            if (tmp == null) return;
            var pos = 0;
            if (!(tmp.HasCharPrefix('>', ref pos) ||
                tmp.HasMentionPrefix(client.CurrentUser, ref pos)) ||
                tmp.Author.IsBot)
                return;
            var context = new SocketCommandContext(client, tmp);
            var result = await commandService.ExecuteAsync(context, pos, services);
            if (!result.IsSuccess)
                Console.WriteLine(result.ErrorReason);
        }

        async Task Ready()
        {

        }
    }
}
