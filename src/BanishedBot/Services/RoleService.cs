using BanishedBot.Models;
using Discord.WebSocket;
using System.Collections.Generic;

namespace BanishedBot.Services
{
    internal class RoleService
    {
        readonly DiscordSocketClient client;

        static List<Raid> Raids { get; set; } = new List<Raid>();

        public RoleService(DiscordSocketClient client)
        {
            this.client = client;
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
}
