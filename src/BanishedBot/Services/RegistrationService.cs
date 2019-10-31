using BanishedBot.Properties;
using BanishedBot.Services;
using BanishedBot.Statics;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Threading;
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
            await RegisterResources();
            await RegisterServices();
            await RegisterModulesAsync();
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
            await CheckFileAsync(nameof(Resources.ZG0_jpg), Resources.ZG0_jpg);
            await CheckFileAsync(nameof(Resources.ZG1_jpg), Resources.ZG1_jpg);
            await CheckFileAsync(nameof(Resources.ZG2_png), Resources.ZG2_png);
            await CheckFileAsync(nameof(Resources.AQR0_jpg), Resources.AQR0_jpg);
            await CheckFileAsync(nameof(Resources.AQR1_jpg), Resources.AQR1_jpg);
            await CheckFileAsync(nameof(Resources.MCT0_jpg), Resources.MCT0_jpg);
            await CheckFileAsync(nameof(Resources.MC0_jpg), Resources.MC0_jpg);
            await CheckFileAsync(nameof(Resources.MC1_jpg), Resources.MC1_jpg);
            await CheckFileAsync(nameof(Resources.MC2_webp), Resources.MC2_webp);
            await CheckFileAsync(nameof(Resources.ONY0_jpg), Resources.ONY0_jpg);
            await CheckFileAsync(nameof(Resources.ONY1_jpg), Resources.ONY1_jpg);
            await CheckFileAsync(nameof(Resources.BWL0_jpg), Resources.BWL0_jpg);
            var set = Resources.ResourceManager.GetResourceSet(Thread.CurrentThread.CurrentCulture, false, true);
            foreach (DictionaryEntry name in set)
                Strings.Resources.Add(name.Key.ToString());
        }

        async Task CheckFileAsync(string filename, byte[] file)
        {
            var path = Paths.PathBuilder(filename);
            if (!File.Exists(path))
                await File.WriteAllBytesAsync(path, file);
        }
    }
}
