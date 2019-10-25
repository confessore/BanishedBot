using BanishedBot.Statics;
using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
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
            var msg = await message.GetOrDownloadAsync();
            foreach (var rctn in msg.Reactions)
            {
                foreach (var user in await msg.GetReactionUsersAsync(rctn.Key, 100).FirstOrDefault())
                {
                    if (user.Username != client.CurrentUser.Username)
                    {
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
