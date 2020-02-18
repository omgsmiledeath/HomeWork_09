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
        static ITelegramBotClient botClient;
        public static void Main(string[] args)
        {

            string token = "1095469268:AAE7JJq1Xb0MrVQvcAKqbfsQHKJUSl2LgGQ";

            var proxy = new WebProxy()
            {
                Address = new Uri($"http://162.243.108.129:3128"),
                UseDefaultCredentials = false,
            };

            var httpCliendHandler = new HttpClientHandler() { Proxy = proxy };
            HttpClient hc = new HttpClient(httpCliendHandler);

            botClient = new TelegramBotClient(token, hc) ;
            var me = botClient.GetMeAsync().Result;
             Console.WriteLine(me.Username);
                
                botClient.OnMessage += HanldeMessage;
                botClient.StartReceiving();

            Console.ReadKey();
        }

        static async void HanldeMessage(object sender, Telegram.Bot.Args.MessageEventArgs e )
        {
            string text = $"{DateTime.Now.ToLongTimeString()}: {e.Message.Chat.FirstName} {e.Message.Chat.Id} {e.Message.Text}";
            //Console.WriteLine(e.Message.Text);
            var chatId = e.Message.Chat.Id;
           await botClient.SendTextMessageAsync(chatId, "Дратути");
        }
    }

         
    }

