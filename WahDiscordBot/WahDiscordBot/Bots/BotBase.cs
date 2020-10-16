using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace WahDiscordBot.Bots
{
    public class BotBase
    {
        protected DiscordSocketClient _client;
        protected CommandService _commands;
        protected IServiceProvider _services;
        protected string serverWelcomeMessage = "Welcome, {0}!"; // default

        /// <summary>
        /// Connect to voice chanal.
        /// </summary>
        /// <param name="voiceChannel"></param>
        /// <returns></returns>
        private async Task ConnectToVoice(SocketVoiceChannel voiceChannel)
        {
            if (voiceChannel == null)
                return;

            Console.WriteLine($"Connecting to channel {voiceChannel.Name}");
            var connection = await voiceChannel.ConnectAsync();
            Console.WriteLine($"Connected to channel {voiceChannel.Name}");
        }

        /// <summary>
        /// Welcom message for a joined user and set it to the lowest rank too.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="welcomeMessage"></param>
        /// <returns></returns>
        public async Task AnnounceJoinedUser(SocketGuildUser user) //Welcomes the new user
        {
            if (user.IsBot || user.IsWebhook)
                return;
            // write the welcome message:
            var channel = _client.GetChannel(331881809191108608) as SocketTextChannel; // Gets the channel to send the message in
            await channel.SendMessageAsync(string.Format(serverWelcomeMessage, user.Username)); //Welcomes the new user
                                                                                          // promote to the lowest rang:
            var role = channel.Guild.GetRole(546809513542549523); // lowest role ID
            await channel.SendMessageAsync(string.Format("{0} elő lett léptetve a következő rangra: {1}", user.Username, role.Name));
            await user.AddRoleAsync(role);
        }

        /// <summary>
        /// Handle the command messages.
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        protected Task CommandHandler(SocketMessage arg)
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

        /// <summary>
        /// Log messages.
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        protected Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        /// <summary>
        /// A voice channel join event checker.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="state1"></param>
        /// <param name="state2"></param>
        /// <returns></returns>
        protected async Task OnVoiceStateUpdated(SocketUser user, SocketVoiceState state1, SocketVoiceState state2)
        {
            // Check if this was a non-bot user joining a voice channel
            if (user.IsBot)
                return;

            if (state1.VoiceChannel == null && state2.VoiceChannel != null)
            {
                ConnectToVoice(state2.VoiceChannel).Start();
            }
        }
    }
}
