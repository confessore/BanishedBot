using BanishedBot.Enums;
using BanishedBot.Properties;
using BanishedBot.Statics;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections;
using System.Linq;
using System.Resources;
using System.Threading;
using System.Threading.Tasks;

namespace BanishedBot.Discord.Modules
{
    public class CommandModule : ModuleBase<SocketCommandContext>
    {
        readonly IServiceProvider services;
        readonly DiscordSocketClient client;
        readonly CommandService commands;

        public CommandModule(
            IServiceProvider services,
            DiscordSocketClient client,
            CommandService commands)
        {
            this.services = services;
            this.client = client;
            this.commands = commands;
            random = new Random();
        }

        readonly Random random;

        [Command("help")]
        [Summary("all: displays available commands" +
            "\n >help")]
        async Task HelpAsync()
        {
            await RemoveCommandMessageAsync();
            var embedBuilder = new EmbedBuilder();
            foreach (var command in await commands.GetExecutableCommandsAsync(Context, services))
                embedBuilder.AddField(command.Name, command.Summary ?? "no summary available");
            await ReplyAsync("here's a list of commands and their summaries: ", false, embedBuilder.Build());
        }

        [Command("insult")]
        [Summary("all: got 'em" +
            "\n >insult")]
        async Task InsultAsync()
        {
            await RemoveCommandMessageAsync();
            await ReplyAsync("your mother");
        }

        [Command("nick")]
        [Summary("all: change your nick" +
            "\n >nick 'your nick here'")]
        async Task NickAsync([Remainder] string name)
        {
            await RemoveCommandMessageAsync();
            await client.GetGuild(Context.Guild.Id).GetUser(Context.User.Id).ModifyAsync(x => x.Nickname = name);
        }

        [RequireUserPermission(GuildPermission.Administrator)]
        [Command("create")]
        [Summary("officers: creates a new event" +
            "\n >create 'minute' 'hour' 'day' 'month' 'year' 'raid' 'optional' 'trash'" +
            "\n >create 0 20 25 10 2019 2" +
            "\n >create 0 20 25 10 2019 2 false" +
            "\n >create 0 20 25 10 2019 2 false true" +
            "\n raid: 0 ZG, 1 AQ20, 2 MC, 3 BWL, 4 DoN, 5 AQ40, 6 Naxx")]
        async Task CreateAsync(int minute, int hour, int day, int month, int year, Instance raid, bool optional = false, bool trash = false)
        {
            await RemoveCommandMessageAsync();
            var dt = DateTime.Parse($"{month}/{day}/{year} {hour}:{minute}").ToString("dd/MMMM/yyyy hh:mm tt 'Server Time'");
            int r;
            string path;
            switch (raid)
            {
                case Instance.ZulGurub:
                    r = random.Next(0, 3);
                    path = Paths.PathBuilder(Strings.Resources.FirstOrDefault(x => x.Contains($"ZG{r}")));
                    break;
                case Instance.AhnQirajRuins:
                    r = random.Next(0, 2);
                    path = Paths.PathBuilder(Strings.Resources.FirstOrDefault(x => x.Contains($"AQR{r}")));
                    break;
                case Instance.MoltenCore:
                    if (!trash)
                    {
                        r = random.Next(0, 3);
                        path = Paths.PathBuilder(Strings.Resources.FirstOrDefault(x => x.Contains($"MC{r}")));
                    }
                    else
                        path = Paths.PathBuilder(Strings.Resources.FirstOrDefault(x => x.Contains($"MCT")));
                    break;
                case Instance.Onyxia:
                    r = random.Next(0, 2);
                    path = Paths.PathBuilder(Strings.Resources.FirstOrDefault(x => x.Contains($"ONY{r}")));
                    break;
                case Instance.BlackwingLair:
                    path = Paths.PathBuilder(Strings.Resources.FirstOrDefault(x => x.Contains($"BWL")));
                    break;
                default:
                    path = string.Empty;
                    break;
            }
            //{Regex.Replace(raid.ToString(), "((?<=[a-z])[A-Z]|[A-Z](?=[a-z]))", " $1")}
            var msg = await Context.Channel.SendFileAsync(path, $"{(optional ? "Optional" : string.Empty)} {raid} {(trash ? "Trash Farm" : string.Empty)} {dt}\nreact with a role to singup");
            foreach (var reaction in Strings.Reactions)
                await msg.AddReactionAsync(Guild.Emotes.FirstOrDefault(x => x.Name.ToLower() == reaction.ToLower()));
        }

        async Task RemoveCommandMessageAsync() =>
            await client.GetGuild(Context.Guild.Id).GetTextChannel(Context.Message.Channel.Id).DeleteMessageAsync(Context.Message);

        Emote GetEmote(string name) =>
            Guild.Emotes.Where(x => x.Name.ToLower() == name.ToLower()).FirstOrDefault();

        SocketGuild Guild =>
            client.GetGuild(Context.Guild.Id);
    }
}
