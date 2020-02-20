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
using MihaZupan;

namespace HomeWork_09
{
    class Program
    {
        static TelegramBotClient botClient;
        static List<TelegramUser> users;
        public static void Main(string[] args)
        {

            string token = "1095469268:AAFhHfoGHRsuFTnvua96UqQxbU4fp3Osegw";
            if (!File.Exists("users.txt")) File.Create("users.txt");
            string json1 = File.ReadAllText("users.txt");

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            users = JsonConvert.DeserializeObject<List<TelegramUser>>(json1);
            if (users == null) users =  new List<TelegramUser>();
            //var proxy = new HttpToSocks5Proxy($"96.44.133.110", 58690); 
            var proxy = new WebProxy()
            {
                Address = new Uri($"http://45.177.16.129:999"),
                UseDefaultCredentials = false,
            };

            //var proxy = new HttpToSocks5Proxy(new[] {
            //new ProxyInfo("tor-proxy.com", 1080),
            //new ProxyInfo("random-socks.com", 1090),
            //new ProxyInfo("tor-proxy.com", 1080)
            //});

            //proxy.ResolveHostnamesLocally = true;
            var httpCliendHandler = new HttpClientHandler() { Proxy = proxy };
            HttpClient hc = new HttpClient(httpCliendHandler);
            
            botClient = new TelegramBotClient(token,hc);
            var u = botClient.TestApiAsync().Result;
            Console.WriteLine(u);


            botClient.OnMessage += HanldeMessage;
            botClient.StartReceiving();


            Console.ReadKey();
            string json = JsonConvert.SerializeObject(users);
            File.WriteAllText("users.txt", json);
        }
        
        static void HanldeMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            string text = $"{DateTime.Now.ToLongTimeString()}: {e.Message.Chat.FirstName} {e.Message.Chat.Id} {e.Message.Text}";
            botClient.SendTextMessageAsync(e.Message.Chat.Id, "Дратути");
            var user = new TelegramUser(e.Message.Chat.Id, e.Message.Chat.FirstName);
            
            if (!users.Contains(user)) users.Add(user);
            users[users.IndexOf(user)].addMessage($"{e.Message.Text}");

            if (e.Message.Type == Telegram.Bot.Types.Enums.MessageType.Document)
            {
                Console.WriteLine(e.Message.Document.FileId);
                Console.WriteLine(e.Message.Document.FileName);
                Console.WriteLine(e.Message.Document.FileSize);

                DownLoad(e.Message.Document.FileId, e.Message.Document.FileName,e.Message.Chat.FirstName);
            }
        }

        static async void DownLoad(string field,string path,string name)
        {
            var file = await botClient.GetFileAsync(field);
            FileStream fs = new FileStream(name + @"\" + path, FileMode.Create);
            await botClient.DownloadFileAsync(file.FilePath, fs);
            fs.Close();
            fs.Dispose();
        }
    }

         
    }

