using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace WahDiscordBot.Modules
{
    public class TextBaseCommands : ModuleBase<SocketCommandContext>
    {
        /// For the poll command:
        /// Help: http://www.fileformat.info/search/
        /// Example search: REGIONAL INDICATOR SYMBOL LETTER A
        private static readonly Emoji[] _awailableReactions = new Emoji[]
                    {
                        new Emoji("\uD83C\uDDE6"), // a
                        new Emoji("\uD83C\uDDE7"), // b
                        new Emoji("\uD83C\uDDE8"), // c
                        new Emoji("\uD83C\uDDE9"), // d
                        new Emoji("\uD83C\uDDEA"), // e
                        new Emoji("\uD83C\uDDEB"), // f
                        new Emoji("\uD83C\uDDEC"), // g
                    }; // If you need more just add some here...

        [Command("help")]
        public async Task Help()
        {
            StringBuilder message = new StringBuilder();
            message.AppendLine("A bot által felismert parancsok a következők:");
            message.AppendLine("!help       Kiírja ezt a szöveget.");
            message.AppendLine("!ping       Egy jó kis ping pong mecs mehet?");
            message.AppendLine("!echo       Megismétli a beírt szöveget.");
            message.AppendLine("!poll       Szavazást lehet kiírni vele, melyet a következő formában kell megadni: Kérdés?,válasz1,válasz2,... Legalább 2 válasz lehetőség megadása kötelező és maximum " + _awailableReactions.Count() + " opció lehetséges.");

            await ReplyAsync(message.ToString());
        }

        [Command("ping")]
        public async Task Ping()
        {
            await ReplyAsync("pong");
        }

        [Command("echo")]
        public async Task Echo([Remainder] string text)
        {
            await ReplyAsync(text, true);
        }

        [Command("respects"), Alias("F")]
        [RequireBotPermission(GuildPermission.AddReactions)]
        public async Task Respects(SocketGuildUser user)
        {
            var emoji = new Emoji("\uD83C\uDDEB");
            string message = $"Press F to pay respects to {user.Mention}:";
            var sent = await Context.Channel.SendMessageAsync(message);
            await sent.AddReactionAsync(emoji);
        }

        [Command("poll")]
        public async Task Poll([Remainder] string questionAndOptions)
        {

            string[] parts = questionAndOptions.Split(',');
            if (parts.Length < 3)
                await ReplyAsync("Rossz a kérdés megfogalmazása, használd a !help parancsot további információkért.");
            else if(parts.Length > _awailableReactions.Count()+1)
                await ReplyAsync("Túl sok válaszlehetőség max " + _awailableReactions.Count() + " lehet!");
            else
            {
                try
                {
                    string question = parts[0];
                    string[] answerOptions = parts.ToList().GetRange(1, parts.Length-1).ToArray();

                    

                    List<Emoji> usedEmotes = new List<Emoji>();

                    var emoji = Context.Client.Guilds.SelectMany(g => g.Emotes).ToList();
                    Console.WriteLine(emoji.Count);

                    StringBuilder answers = new StringBuilder();
                    for (int i = 0; i < answerOptions.Length; i++)
                    {
                        answers.AppendLine(_awailableReactions[i] + "  :  " + answerOptions[i]);
                        usedEmotes.Add(_awailableReactions[i]);
                    }
                    
                    var embed = new EmbedBuilder{ Color = Color.Gold, Title = question, Description = answers.ToString() };
                    IUserMessage sent = await ReplyAsync("", false, embed.Build());
                    await sent.AddReactionsAsync(usedEmotes.ToArray());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.StackTrace);
                    await ReplyAsync("Üzemzavar bocsi....");
                }
            }      
        }
    }
}
