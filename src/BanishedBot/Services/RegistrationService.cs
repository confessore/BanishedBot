using BanishedBot.Discord.Services;
using BanishedBot.Properties;
using BanishedBot.Services;
using BanishedBot.Statics;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace NashorMatch.Discord.Services
{
    internal class RegistrationService
    {
        readonly IServiceProvider services;
        readonly CommandService commands;

        public RegistrationService(
            IServiceProvider services,
            CommandService commands)
        {
            this.services = services;
            this.commands = commands;
        }

        public async Task IntializeRegistrationsAsync()
        {
            await RegisterServices();
            await RegisterModulesAsync();
            await RegisterResources();
            Console.WriteLine("registration completed!");
        }

        async Task RegisterModulesAsync()
        {
            Console.WriteLine("registering modules...");
            await commands.AddModulesAsync(
                Assembly.GetEntryAssembly(),
                services);
        }

        Task RegisterServices()
        {
            Console.WriteLine("registering services...");
            services.GetRequiredService<BaseService>();
            services.GetRequiredService<EventService>();
            services.GetRequiredService<ChannelService>();
            services.GetRequiredService<RoleService>();
            return Task.CompletedTask;
        }

        async Task RegisterResources()
        {
            Console.WriteLine("registering resources...");
            if (!File.Exists(Paths.ZG))
                await File.WriteAllBytesAsync(Paths.ZG, Resources.ZG);
            if (!File.Exists(Paths.AQR))
                await File.WriteAllBytesAsync(Paths.AQR, Resources.AQR);
            if (!File.Exists(Paths.MC))
                await File.WriteAllBytesAsync(Paths.MC, Resources.MC);
            if (!File.Exists(Paths.MCT))
                await File.WriteAllBytesAsync(Paths.MCT, Resources.MCT);
        }
    }
}
