using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;

namespace HomeWork_09
{
    class Program
    {
        static TelegramBotClient botClient;
        public static void Main(string[] args)
        {

            string token = "1095469268:AAHx8NoO7r5iwcPsbeqGIQC70mTakIGkk9M";

            var proxy = new WebProxy()
            {
                Address = new Uri($"http://207.154.231.216:3128"),
                UseDefaultCredentials = false
            };

            var httpCliendHandler = new HttpClientHandler() { Proxy = proxy };
            HttpClient hc = new HttpClient(httpCliendHandler);

            botClient = new TelegramBotClient(token,hc);
            // var me = botClient.GetMeAsync().Result;
            // Console.WriteLine(me.Username);

                botClient.OnMessage += HanldeMessage;
                botClient.StartReceiving();

            Console.ReadKey();
        }

        static void HanldeMessage(object sender, Telegram.Bot.Args.MessageEventArgs e )
        {
            //Console.WriteLine(e.Message.Text);
            var chatId = e.Message.Chat.Id;
            botClient.SendTextMessageAsync(chatId, "Дратути");
        }
    }

         
    }

