using BanishedBot.Enums;
using BanishedBot.Properties;
using BanishedBot.Statics;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using NashorMatch.Discord.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        }

        [Command("help")]
        [Summary(">help \n all: displays available commands")]
        async Task HelpAsync()
        {
            await RemoveCommandMessageAsync();
            var embedBuilder = new EmbedBuilder();
            foreach (var command in await commands.GetExecutableCommandsAsync(Context, services))
                embedBuilder.AddField(command.Name, command.Summary ?? "no summary available");
            await ReplyAsync("here's a list of commands and their summaries: ", false, embedBuilder.Build());
        }

        [Command("insult")]
        [Summary(">insult \n all: got 'em")]
        async Task InsultAsync()
        {
            await RemoveCommandMessageAsync();
            await ReplyAsync("your mother");
        }

        [Command("nick")]
        [Summary(">nick 'your nick here' \n all: change your nick")]
        async Task NickAsync([Remainder] string name)
        {
            await RemoveCommandMessageAsync();
            await client.GetGuild(Context.Guild.Id).GetUser(Context.User.Id).ModifyAsync(x => x.Nickname = name);
        }

        /*[Command("druid")]
        [Summary(">druid \n all: change your class to druid")]
        async Task DruidAsync()
        {
            await RemoveCommandMessageAsync();
            await UpdateWithRole("Druid");
        }

        [Command("mage")]
        [Summary(">mage \n all: change your class to mage")]
        async Task MageAsync()
        {
            await RemoveCommandMessageAsync();
            await UpdateWithRole("Mage");
        }

        [Command("priest")]
        [Summary(">priest \n all: change your class to priest")]
        async Task PriestAsync()
        {
            await RemoveCommandMessageAsync();
            await UpdateWithRole("Priest");
        }

        [Command("shaman")]
        [Summary(">shaman \n all: change your class to shaman")]
        async Task ShamanAsync()
        {
            await RemoveCommandMessageAsync();
            await UpdateWithRole("Shaman");
        }

        [Command("hunter")]
        [Summary(">hunter \n all: change your class to hunter")]
        async Task HunterAsync()
        {
            await RemoveCommandMessageAsync();
            await UpdateWithRole("Hunter");
        }

        [Command("rogue")]
        [Summary(">rogue \n all: change your class to rogue")]
        async Task RogueAsync()
        {
            await RemoveCommandMessageAsync();
            await UpdateWithRole("Rogue");
        }

        [Command("warlock")]
        [Summary(">warlock \n all: change your class to warlock")]
        async Task WarlockAsync()
        {
            await RemoveCommandMessageAsync();
            await UpdateWithRole("Warlock");
        }

        [Command("warrior")]
        [Summary(">warrior \n all: change your class to warrior")]
        async Task WarriorAsync()
        {
            await RemoveCommandMessageAsync();
            await UpdateWithRole("Warrior");
        }*/

        [Command("create")]
        [Summary(">create 'minute' 'hour' 'day' 'month' 'year' 'raid'" +
            "\n raid: 0 ZG, 1 AQ20, 2 MC, 3 BWL, 4 DoN, 5 AQ40, 6 Naxx" +
            "\n all: creates a new event")]
        async Task CreateAsync(int minute, int hour, int day, int month, int year, Raid raid)
        {
            await RemoveCommandMessageAsync();
            var dt = DateTime.Parse($"{month}/{day}/{year} {hour}:{minute}").ToString("dd/MMMM/yyyy hh:mm tt 'Server Time'");
            string path;
            switch (raid)
            {
                case Raid.ZulGurub:
                    path = Paths.ZG;
                    break;
                case Raid.AhnQirajRuins:
                    path = Paths.AQR;
                    break;
                case Raid.MoltenCore:
                    path = Paths.MC;
                    break;
                default:
                    path = string.Empty;
                    break;
            }
            var msg = await Context.Channel.SendFileAsync(path, $"{raid} {dt} \n react with a role to singup");
            foreach (var reaction in Strings.Reactions)
                await msg.AddReactionAsync(Guild.Emotes.FirstOrDefault(x => x.Name.ToLower() == reaction.ToLower()));
        }

        async Task RemoveCommandMessageAsync() =>
            await client.GetGuild(Context.Guild.Id).GetTextChannel(Context.Message.Channel.Id).DeleteMessageAsync(Context.Message);

        Emote GetEmote(string name) =>
            Guild.Emotes.Where(x => x.Name.ToLower() == name.ToLower()).FirstOrDefault();

        SocketGuildUser GetGuildUser(string name) =>
            client.GetGuild(Context.Guild.Id).Users.Where(x => (x.Nickname ?? x.Username).ToLower() == name.ToLower()).FirstOrDefault();

        SocketGuild Guild =>
            client.GetGuild(Context.Guild.Id);

        SocketGuildUser SocketGuildUser =>
            client.GetGuild(Context.Guild.Id).GetUser(Context.User.Id);

        bool IsVerified =>
            client.GetGuild(Context.Guild.Id).GetUser(Context.User.Id).Roles.Any(x => x.Name.ToLower().Contains("verified"));
    }
}
