using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace WahDiscordBot.Bots
{
    public class BotBase
    {
        protected Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        protected DiscordSocketClient _client;
        protected CommandService _commands;
        protected IServiceProvider _services;
    }
}
