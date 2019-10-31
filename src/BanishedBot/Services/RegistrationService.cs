using BanishedBot.Properties;
using BanishedBot.Services;
using BanishedBot.Statics;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace BanishedBot.Discord.Services
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
            if (!File.Exists(Paths.ZG(0)))
                await File.WriteAllBytesAsync(Paths.ZG(0), Resources.ZG0);
            if (!File.Exists(Paths.ZG(1)))
                await File.WriteAllBytesAsync(Paths.ZG(1), Resources.ZG1);
            if (!File.Exists(Paths.ZG(2)))
                await File.WriteAllBytesAsync(Paths.ZG(2), Resources.ZG2);

            if (!File.Exists(Paths.AQR(0)))
                await File.WriteAllBytesAsync(Paths.AQR(0), Resources.AQR0);
            if (!File.Exists(Paths.AQR(1)))
                await File.WriteAllBytesAsync(Paths.AQR(0), Resources.AQR1);

            if (!File.Exists(Paths.MCT(0)))
                await File.WriteAllBytesAsync(Paths.MCT(0), Resources.MCT0);

            if (!File.Exists(Paths.MC(0)))
                await File.WriteAllBytesAsync(Paths.MC(0), Resources.MC0);
            if (!File.Exists(Paths.MC(1)))
                await File.WriteAllBytesAsync(Paths.MC(1), Resources.MC1);
            if (!File.Exists(Paths.MC(2)))
                await File.WriteAllBytesAsync(Paths.MC(2), Resources.MC2);

            if (!File.Exists(Paths.ONY(0)))
                await File.WriteAllBytesAsync(Paths.ONY(0), Resources.ONY0);

            if (!File.Exists(Paths.BWL(0)))
                await File.WriteAllBytesAsync(Paths.BWL(0), Resources.BWL0);
        }
    }
}
