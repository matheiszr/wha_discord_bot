using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace WahDiscordBot.Bots
{
    public class Glados : BotBase
    {
        // bot own login token:
        private static readonly string token = "NzY1NjE5MDMzMjYwNDI1MzEz.X4XcSg.H4REPeOGASDirw4yvXtC-asPIR0";
        // text messages:
        private static readonly string WelcomeNewUserMessage = "Á szóval {0} te leszel az új teszt alanyom? Nagyszerű üdv köreinkben, kezdhetjük is a tesztelést.";

        public async Task RunAsync()
        {
            // initialize:
            _client = new DiscordSocketClient();
            _commands = new CommandService();
            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .BuildServiceProvider();

            _client.MessageReceived += CommandHandler;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
            _client.Log += Log;
            _client.UserJoined += UserJoined;

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();
            await Task.Delay(-1);
        }

        private Task UserJoined(SocketGuildUser gUser)
        {
            if (gUser.IsBot || gUser.IsWebhook)
                return Task.CompletedTask;
            
            // TODO do something...

            return Task.CompletedTask;
        }

        private Task CommandHandler(SocketMessage arg)
        {
            // bots don't reacts for each other:
            var message = arg as SocketUserMessage;
            var context = new SocketCommandContext(_client, message);
            if (message.Author.IsBot) 
                return Task.CompletedTask;

            int argPos = 0;
            if (message.HasStringPrefix("!", ref argPos))
            {
                message.DeleteAsync();
                var result = _commands.ExecuteAsync(context, argPos, _services);
                if (!result.IsCompleted) 
                    Console.WriteLine(result.Exception);
            }

            return Task.CompletedTask;
        }
    }
}
