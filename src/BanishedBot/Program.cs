using BanishedBot.Discord.Services;
using BanishedBot.Services;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using NashorMatch.Discord.Services;
using System;
using System.Threading.Tasks;

namespace BanishedBot
{
    class Program
    {
        IServiceProvider services;
        DiscordSocketClient client;

        static void Main(string[] args) =>
            new Program().MainAsync().GetAwaiter().GetResult();

        async Task MainAsync()
        {
            client = new DiscordSocketClient();
            services = ConfigureServices();
            await services.GetRequiredService<RegistrationService>().IntializeRegistrationsAsync();
            await client.LoginAsync(
                TokenType.Bot,
                Environment.GetEnvironmentVariable("BanishedBotDiscordToken"));
            await client.StartAsync();
            await client.SetGameAsync("'>help' for commands");
            await Task.Delay(-1);
        }

        IServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton(client)
                .AddSingleton<BaseService>()
                .AddSingleton<CommandService>()
                .AddSingleton<RegistrationService>()
                .AddSingleton<ChannelService>()
                .AddSingleton<MessageService>()
                .AddSingleton<RoleService>()
                .AddSingleton<EventService>()
                .BuildServiceProvider();
        }
    }
}
