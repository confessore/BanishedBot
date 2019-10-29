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

        static List<Raid> Raids { get; set; } = new List<Raid>();

        public RoleService(DiscordSocketClient client)
        {
            this.client = client;
            //client.ReactionAdded += ReactionAdded;
        }

        SocketGuild Guild =>
            client.Guilds.Where(x => x.Name == Strings.GuildName).FirstOrDefault();

        SocketTextChannel Channel =>
            Guild.TextChannels.Where(x => x.Name == Strings.RoleChannel).FirstOrDefault();

        IMessage Message =>
            Channel.GetMessageAsync(635609999275720705).GetAwaiter().GetResult();

        async Task ReactionAdded(Cacheable<IUserMessage, ulong> message, ISocketMessageChannel channel, SocketReaction reaction)
        {
            if (channel.Name == Channel.Name)
            {
                var msg = await message.GetOrDownloadAsync();
            }
            /*if (reaction.UserId != client.CurrentUser.Id)
            {
                var msg = await message.GetOrDownloadAsync();
                var content = msg.Content.Split(" ").ToList();
                var index = content.IndexOf("Server");
                var dt = $"{content[index - 3]} {content[index - 2]} {content[index - 1]}";
                var format = "dd/MMMM/yyyy hh:mm tt";
                var raid = new Raid
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
                            if (!Strings.Reactions.Contains(rctn.Key.Name.ToLower()))
                            {
                                await msg.RemoveReactionAsync(rctn.Key, user);
                                continue;
                            }
                            var u = Guild.Users.FirstOrDefault(x => x.Id == user.Id);
                            var raider = new Raider
                            {
                                User = user,
                                Name = u.Nickname ?? u.Username,
                                Role = rctn.Key
                            };
                            var existing = raid.Raiders.Any(x => x.Name == raider.Name);
                            if (existing)
                            {
                                raid.Raiders.Add(raider);
                                var emotes = new List<IEmote>();
                                foreach (var rdr in raid.Raiders.Where(x => x.Name == raider.Name).ToList())
                                {
                                    raid.Raiders.Remove(rdr);
                                    emotes.Add(rdr.Role);
                                }
                                await msg.RemoveReactionsAsync(user, emotes.ToArray());
                            }
                            else
                                raid.Raiders.Add(raider);
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
                if (Raids.Any(x => x.DateTime == raid.DateTime))
                    Raids.FirstOrDefault(x => x.DateTime == raid.DateTime).Raiders = raid.Raiders;
                else
                    Raids.Add(raid);

                Console.Write(Raids.Count);
            }*/
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
            await client.GetGuild(guild.Id).GetUser(user.Id).AddRoleAsync(GetGuildRole(guild, name));
        }

        SocketRole GetGuildRole(SocketGuild guild, string name) =>
            client.GetGuild(guild.Id).Roles.Where(x => x.Name.ToLower() == name.ToLower()).FirstOrDefault();
    }
}
