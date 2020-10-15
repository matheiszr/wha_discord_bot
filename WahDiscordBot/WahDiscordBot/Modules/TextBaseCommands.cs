using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace WahDiscordBot.Modules
{
    public class TextBaseCommands : ModuleBase<SocketCommandContext>
    {
        [Command("help")]
        public async Task Help()
        {
            StringBuilder message = new StringBuilder();
            message.AppendLine("A bot által felismert parancsok a következők:");
            message.AppendLine("!help       Kiírja ezt a szöveget.");
            message.AppendLine("!ping       Egy jó kis ping pong mecs mehet?");
            message.AppendLine("!echo       Megismétli a beírt szöveget.");
            message.AppendLine("!poll       Szavazást lehet kiírni vele, melyet a következő formában kell megadni: Kérdés?,válasz1,válasz2,... Legalább 2 válasz lehetőség megadása kötelező.");

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

        [Command("poll")]
        public async Task Poll([Remainder] string questionAndOptions)
        {
            string[] parts = questionAndOptions.Split(',');
            if (parts.Length < 3)
                await ReplyAsync("Rossz a kérdés megfogalmazása, használd a !help parancsot további információkért.");
            else if(parts.Length > 8)
                await ReplyAsync("Túl sok válaszlehetőség.");
            else
            {
                try
                {
                    string question = parts[0];
                    Console.WriteLine("Question: " + question);
                    string[] answerOptions = parts.ToList().GetRange(1, parts.Length-1).ToArray();

                    //List<IEmote> emotesOnServer = Context.Guild.Users..ToList<IEmote>();
                    //Console.WriteLine("Emotes size: " + emotesOnServer.Count);
                    List<IEmote> reactionEmotes = new List<IEmote>();

                    string[] emojiNames = new string[] { "grinning", "smiley", "smile", "grin", "relaxed", "blush", "innocent"};

                    var emoji = Context.Client.Guilds.SelectMany(g => g.Emotes).ToList();
                    Console.WriteLine(emoji.Count);

                    StringBuilder answers = new StringBuilder();
                    for (int i = 0; i < answerOptions.Length; i++)
                    {
                        answers.AppendLine(":"+emojiNames[i]+":" + " : " + answerOptions[i]);
                        //IEmote e = Context.Guild.Emotes.First(x => x.Name == emojiNames[i]);
                        //if (e == null)
                        //    reactionEmotes.Add(e);
                        //else
                            reactionEmotes.Add(new Emoji(emojiNames[i]));
                    }
                    var embed = new EmbedBuilder{ Color = Color.Gold, Title = question, Description = answers.ToString() };
                    IUserMessage sent = await ReplyAsync("", false, embed.Build());
                    await sent.AddReactionsAsync(reactionEmotes.ToArray());
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
