using BanishedBot.Enums;
using BanishedBot.Models;
using BanishedBot.Statics;
using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace BanishedBot.Services
{
    internal class RoleService
    {
        readonly DiscordSocketClient client;

        public RoleService(DiscordSocketClient client)
        {
            this.client = client;
            client.ReactionAdded += ReactionAdded;
        }

        SocketGuild Guild =>
            client.Guilds.Where(x => x.Id == 560050209204207636).FirstOrDefault();

        SocketTextChannel Channel =>
            Guild.TextChannels.Where(x => x.Id == 622703508944060416).FirstOrDefault();

        IMessage Message =>
            Channel.GetMessageAsync(635609999275720705).GetAwaiter().GetResult();

        async Task ReactionAdded(Cacheable<IUserMessage, ulong> message, ISocketMessageChannel channel, SocketReaction reaction)
        {
            if (reaction.UserId != client.CurrentUser.Id)
            {
                var msg = await message.GetOrDownloadAsync();
                var content = msg.Content.Split(" ").ToList();
                var index = content.IndexOf("Server");
                var dt = $"{content[index - 3]} {content[index - 2]} {content[index - 1]}";
                var format = "dd/MMMM/yyyy hh:mm tt";
                var raid = new Models.Raid
                {
                    Name = (Enums.Raid)Enum.Parse(typeof(Enums.Raid), content.Contains("Optional") ? content[1] : content[0]),
                    DateTime = DateTime.ParseExact(dt, format, CultureInfo.InvariantCulture)
                };
                foreach (var rctn in msg.Reactions)
                {
                    foreach (var user in await msg.GetReactionUsersAsync(rctn.Key, 100).FirstOrDefault())
                    {
                        if (user.Id != client.CurrentUser.Id)
                        {
                            var u = Guild.Users.FirstOrDefault(x => x.Id == user.Id);
                            var raider = new Raider
                            {
                                Name = u.Nickname ?? u.Username,
                                Role = rctn.Key
                            };
                            raid.Raiders.Add(raider);
                            if (!Strings.Reactions.Contains(rctn.Key.Name.ToLower()))
                                await msg.RemoveReactionAsync(rctn.Key, user);
                            var role = ParseRole(rctn.Key.Name);
                            if (Strings.Roles.Contains(role))
                            {
                                var tmp = Guild.Users.FirstOrDefault(x => x.Id == user.Id);
                                if (!tmp.Roles.Contains(GetGuildRole(Guild, role)))
                                    await ModifyRole(Guild, tmp, role);
                            }
                        }
                    }
                }
                Console.WriteLine(raid.Name);
                Console.WriteLine(raid.DateTime);
                foreach (var raider in raid.Raiders)
                    Console.WriteLine($"{raider.Name} {raider.Role.Name}");
            }
        }

        string ParseRole(string reaction)
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

        async Task ModifyRole(SocketGuild guild, SocketGuildUser user, string role) =>
                await UpdateWithRole(guild, user, role);

        async Task UpdateWithRole(SocketGuild guild, SocketGuildUser user, string name)
        {
            var roles = client.GetGuild(guild.Id).GetUser(user.Id).Roles;
            foreach (var role in roles)
            {
                if (Strings.Roles.Contains(role.Name.ToLower()))
                    await client.GetGuild(guild.Id).GetUser(user.Id).RemoveRoleAsync(role);
            }
            Console.WriteLine($"{user.Nickname ?? user.Username} {name}");
            await client.GetGuild(guild.Id).GetUser(user.Id).AddRoleAsync(GetGuildRole(guild, name));
        }

        SocketRole GetGuildRole(SocketGuild guild, string name) =>
            client.GetGuild(guild.Id).Roles.Where(x => x.Name.ToLower() == name.ToLower()).FirstOrDefault();
    }
}
