using BanishedBot.Models;
using BanishedBot.Services;
using BanishedBot.Statics;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BanishedBot.Discord.Services
{
    internal class EventService
    {
        readonly IServiceProvider services;
        readonly DiscordSocketClient client;
        readonly BaseService baseService;
        readonly CommandService commandService;
        readonly ChannelService channelService;
        readonly MessageService messageService;

        public EventService(
            IServiceProvider services,
            DiscordSocketClient client,
            BaseService baseService,
            CommandService commandService,
            ChannelService channelService,
            MessageService messageService)
        {
            this.services = services;
            this.client = client;
            this.baseService = baseService;
            this.commandService = commandService;
            this.channelService = channelService;
            this.messageService = messageService;
            client.Ready += Ready;
            client.Disconnected += Disconnected;
            client.MessageReceived += MessageReceived;
            client.ReactionAdded += ReactionAdded;
        }

        async Task Ready()
        {
            await channelService.CheckChannelsAsync();
            await messageService.CheckMessages();
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

        async Task ReactionAdded(Cacheable<IUserMessage, ulong> message, ISocketMessageChannel channel, SocketReaction reaction)
        {
            if (channel.Name == Strings.RoleChannel)
            {
                await messageService.CheckMessages();
                var msg = await message.GetOrDownloadAsync();
                var raid = new Raid();
                foreach (var rctn in msg.Reactions)
                {
                    foreach (var user in await msg.GetReactionUsersAsync(rctn.Key, 1000).FirstOrDefault())
                    {
                        if (user.Id != client.CurrentUser.Id)
                        {
                            if (!Strings.Reactions.Contains(rctn.Key.Name.ToLower()))
                            {
                                await msg.RemoveReactionAsync(rctn.Key, user);
                                continue;
                            }
                            var role = baseService.ParseRole(rctn.Key.Name);
                            if (Strings.Roles.Contains(role))
                            {
                                var tmp = baseService.Guild.Users.FirstOrDefault(x => x.Id == user.Id);
                                var raider = new Raider
                                {
                                    User = user,
                                    Name = tmp.Nickname ?? tmp.Username,
                                    Role = rctn.Key
                                };
                                var existing = raid.Raiders.Any(x => x.Name == raider.Name);
                                raid.Raiders.Add(raider);
                                if (existing)
                                {
                                    var emotes = new List<IEmote>();
                                    foreach (var rdr in raid.Raiders.Where(x => x.Name == raider.Name).ToList())
                                    {
                                        raid.Raiders.Remove(rdr);
                                        emotes.Add(rdr.Role);
                                    }
                                    await msg.RemoveReactionsAsync(user, emotes.ToArray());
                                }
                                if (tmp != null)
                                {
                                    if (!tmp.Roles.Contains(baseService.GetGuildRole(baseService.Guild, role)))
                                        await baseService.ModifyRole(baseService.Guild, tmp, role);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
