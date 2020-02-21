using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
namespace HomeWork_09
{
    public static class Bot
    {
        static public  TelegramBotClient TelegramBot;

        public static void Start()
        {
            ProxyParser.LoadProxy();
            setBotWithProxy();
            UsersBase.getUsersFromFile();

            Command.Test();

            Console.ReadKey();
        }

        public static void setBotWithProxy()
        {
            Console.WriteLine("Создаем бота");
            var httpCliendHandler = new HttpClientHandler() { Proxy = ProxyParser.ProxyList[0] };
            HttpClient hc = new HttpClient(httpCliendHandler);
            TelegramBot = new TelegramBotClient(getToken(),hc);
        }

        public static void setBot()
        {
            TelegramBot = new TelegramBotClient(getToken());
        }

        private static string getToken()
        {
            return "1095469268:AAFhHfoGHRsuFTnvua96UqQxbU4fp3Osegw";
            //TODO : Сделать чтение из файла
        }
    }
}
