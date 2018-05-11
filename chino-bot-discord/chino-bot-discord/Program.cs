using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace chino_bot_discord
{
    class Program
    {
        public static DiscordSocketClient client;
        public static CommandService commands;
        public static IServiceProvider services;
        private const ulong channelid = 443502244734828556;
        static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();
        public async Task MainAsync()
        {
            client = new DiscordSocketClient();
            commands = new CommandService();
            services = new ServiceCollection().BuildServiceProvider();
            client.MessageReceived += CommandRecieved;
            client.UserJoined += UserJoined;
            client.UserLeft += UserLeft;
            client.Log += Log;
            string token = Environment.GetEnvironmentVariable("Discord");
            await commands.AddModulesAsync(Assembly.GetEntryAssembly());
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();
            await Task.Delay(-1);
        }
        /// <summary>
        /// 何かしらのメッセージの受信
        /// </summary>
        /// <param name="msgParam"></param>
        /// <returns></returns>
        private async Task CommandRecieved(SocketMessage messageParam)
        {
            var chatchannnel = client.GetChannel(channelid) as SocketTextChannel;
            var message = messageParam as SocketUserMessage;
            Console.WriteLine("{0} {1}:{2}", message.Channel.Name, message.Author.Username, message);
            //メッセージがnullの場合
            if (message == null)
                return;
            //発言者がBotの場合無視する
            if (message.Author.IsBot)
                return;
            int argPos = 0;
            //コマンドかどうか判定
            if (!(message.HasCharPrefix('!', ref argPos) || message.HasMentionPrefix(client.CurrentUser, ref argPos)))
                return;
            //普通の返信
            if (message.ToString() == "チノちゃんかわいい")
                await chatchannnel.SendMessageAsync("ありがとうございます");
            var context = new CommandContext(client, message);
            //コマンドを実行
            var result = await commands.ExecuteAsync(context, argPos, services);
            //実行できなかった場合
            if (!result.IsSuccess)
                await context.Channel.SendMessageAsync(result.ErrorReason);
        }
        /// <summary>
        /// ユーザーが会議に参加したときの処理
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task UserJoined(SocketGuildUser user)
        {
            var chatchannnel = client.GetChannel(channelid) as SocketTextChannel;
            string welcome = string.Format("{0}様、いらっしゃいませ！", user.Username);
            await chatchannnel.SendMessageAsync(welcome);
        }
        /// <summary>
        /// ユーザーが会議から抜けたときの処理
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task UserLeft(SocketGuildUser user)
        {
            var chatchannnel = client.GetChannel(channelid) as SocketTextChannel;
            string bye = string.Format("{0}様さようなら、またのお越しをお待ちしております", user.Username);
            await chatchannnel.SendMessageAsync(bye);
        }
        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}
