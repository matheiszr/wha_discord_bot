using Glados = WahDiscordBot.Bots.Glados;

namespace WahDiscordBot
{
    public class Program
    {
        static void Main(string[] args)
        {
            var glados = new Glados();
            glados.RunAsync().GetAwaiter().GetResult();
        }
    }
}
