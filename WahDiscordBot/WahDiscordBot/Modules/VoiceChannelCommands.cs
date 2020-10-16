using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace WahDiscordBot.Modules
{
    public class VoiceChannelCommands : ModuleBase<SocketCommandContext>
    {
        /*
        [Command("join", RunMode = RunMode.Async)]
        public async Task JoinVoiceChannel(IVoiceChannel channel = null)
        {
            try
            {
                channel = channel ?? (Context.User as IGuildUser)?.VoiceChannel;
                if (channel == null)
                {
                    await Context.Channel.SendMessageAsync("Vagy legyél fent hangoson, ha már hívsz vagy add meg hova menjek, ennyit se tudsz?");
                    return;
                }
                var audioClient = await channel.ConnectAsync();
            } 
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }

        }
        */
    }
}
