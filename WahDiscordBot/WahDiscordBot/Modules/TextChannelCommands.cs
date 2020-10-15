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
    /// <summary>
    /// Contains all available commands for the text channels.
    /// </summary>
    public class TextChannelCommands : ModuleBase<SocketCommandContext>
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

        /// <summary>
        /// Write the available command descriptions.
        /// </summary>
        /// <returns>Result of the tasc.</returns>
        [Command("help")]
        public async Task Help()
        {
            StringBuilder message = new StringBuilder();
            message.AppendLine("- Szöveg csatorna parancsok:");
            message.AppendLine("**!help** - Kiírja ezt a szöveget.");
            message.AppendLine("**!ping** - Egy jó kis ping pong mecs mehet?");
            message.AppendLine("**!echo** - Megismétli a beírt szöveget.");
            message.AppendLine("**!respects** - Ród le a tiszteletedet valaki előtt az F-el. Paramétere egy személy @név formában.");
            message.AppendLine("**!wantplay** - Kiírhatsz játék felhívást paramétere egy szöveg, ahol érdemes megadni a játék nevét és időpontját a többiek emottal jelezhetik a részvételt.");
            message.AppendLine("**!poll** - Szavazást lehet kiírni vele, melyet a következő formában kell megadni: Kérdés?,válasz1,válasz2,... Legalább 2 válasz lehetőség megadása kötelező és maximum " + _awailableReactions.Count() + " opció lehetséges.");
            /*
            message.AppendLine("");
            message.AppendLine("- Hang csatorna parancsok:");
            message.AppendLine("**!join** - Csatlakozik a paraméterben kapott hang csatornára. Paraméter megadása: #csatornanév");
            message.AppendLine("**!say** - A paraméter alapján lejátsza a megfelelő voice line-t. További infókért használd a !whatcansay parancsot.");
            message.AppendLine("**!whatcansay** - Leírja milyne paraméterei lehetnek a say parancsnak.");
            */
            var embed = new EmbedBuilder { Color = Color.DarkGrey, Title = "A bot által felismert parancsok a következők:", Description = message.ToString() };

            await ReplyAsync("", false, embed.Build());
        }

        /// <summary>
        /// Just for test.
        /// </summary>
        /// <returns>Result of the tasc.</returns>
        [Command("ping")]
        public async Task Ping()
        {
            await ReplyAsync("pong");
        }

        /// <summary>
        /// Bot say what you write in parameter.
        /// </summary>
        /// <param name="text"></param>
        /// <returns>Result of the tasc.</returns>
        [Command("echo")]
        public async Task Echo([Remainder] string text)
        {
            await ReplyAsync(text);
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

        /// <summary>
        /// Create an embed with a game invite message.
        /// </summary>
        /// <param name="message">Contains the game and the time when you want it play.</param>
        /// <returns>Result of the tasc.</returns>
        [Command("wantplay")]
        public async Task WantPlay([Remainder] string message)
        {
            if (message == null || message.Trim().Length == 0)
                return;

            var up = new Emoji("\uD83D\uDC4D");
            var off = new Emoji("\uD83D\uDC4E");
            string options = up + " - Jövök!" + Environment.NewLine;
            options += off + " - Off!";

            var embed = new EmbedBuilder { Color = Color.Blue, Title = message, Description = options };
            IUserMessage sent = await ReplyAsync("", false, embed.Build());
            await sent.AddReactionsAsync(new[] { up, off });
        }

        /// <summary>
        /// You can create votings with this command.
        /// </summary>
        /// <param name="questionAndOptions"></param>
        /// <returns>Result of the tasc.</returns>
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
