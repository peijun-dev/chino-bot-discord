using Discord.Commands;
using System.Threading.Tasks;

namespace chino_bot_discord
{
    public class Chino : ModuleBase
    {
        [Command("test")]
        public async Task test()
        {
            await ReplyAsync("ok");
        }
        [Command("おはよう")]
        public async Task GoodMorning()
        {
            await ReplyAsync("おはようございます");
        }
    }
}
