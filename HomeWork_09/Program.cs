using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.IO;
namespace HomeWork_09
{
    class Program
    {
        static TelegramBotClient botClient;
        static List<TelegramUser> users;
        public static void Main(string[] args)
        {

            string token = "1095469268:AAFhHfoGHRsuFTnvua96UqQxbU4fp3Osegw";
            string json1 = File.ReadAllText("users.txt");
            

            users = JsonConvert.DeserializeObject<List<TelegramUser>>(json1);
            if (users == null) users =  new List<TelegramUser>();
            var proxy = new WebProxy()
            {
                Address = new Uri($"http://163.172.146.119:8811"),
                UseDefaultCredentials = false,
            };

            var httpCliendHandler = new HttpClientHandler() { Proxy = proxy };
            HttpClient hc = new HttpClient(httpCliendHandler);

            botClient = new TelegramBotClient(token, hc);
            var u = botClient.TestApiAsync().Result;
            Console.WriteLine(u);
            //var me = botClient.GetMeAsync().Result;
            // Console.WriteLine(me.Username);

            botClient.OnMessage += HanldeMessage;
            botClient.StartReceiving();

            

            Console.ReadKey();
            string json = JsonConvert.SerializeObject(users);
            File.WriteAllText("users.txt", json);
        }
        
        static void HanldeMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            string text = $"{DateTime.Now.ToLongTimeString()}: {e.Message.Chat.FirstName} {e.Message.Chat.Id} {e.Message.Text}";
            //Console.WriteLine(e.Message.Text);
            var chatId = e.Message.Chat.Id;
            botClient.SendTextMessageAsync(chatId, "Дратути");
            var user = new TelegramUser(e.Message.Chat.Id, e.Message.Chat.FirstName);
            
            if (!users.Contains(user)) users.Add(user);
            users[users.IndexOf(user)].addMessage($"{user.Name}:{e.Message.Text}");

           
        }
    }

         
    }

