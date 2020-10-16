using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Threading.Tasks;

namespace WahDiscordBot.Bots
{
    public class Glados : BotBase
    {
        // bot own login token:
        private static readonly string token = "NzY1NjE5MDMzMjYwNDI1MzEz.X4XcSg.H4REPeOGASDirw4yvXtC-asPIR0";

        public Glados()
        {
            this.serverWelcomeMessage = "Á szóval {0} te leszel az új teszt alanyom? Nagyszerű! Üdv köreinkben, kezdhetjük is a tesztelést?";
        }

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
            _client.UserJoined += AnnounceJoinedUser;
            //_client.UserVoiceStateUpdated += OnVoiceStateUpdated; // FIXME nem mükszik !!!

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();
            await Task.Delay(-1);
        }

    }
}
