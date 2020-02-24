using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using System.IO;
namespace HomeWork_09
{
    public static class Bot
    {
        /// <summary>
        /// экземпляр телеграм бота
        /// </summary>
        static public  TelegramBotClient TelegramBot;

        /// <summary>
        /// Метод по запуску функционирования бота
        /// </summary>
        public static void Start()
        {
            ProxyParser.LoadProxy();
            setBotWithProxy();
            UsersBase.getUsersFromFile();

            Command.Start();

            Console.ReadKey();
        }

        /// <summary>
        /// Задание настроек боту с использованием прокси
        /// </summary>
        public static void setBotWithProxy()
        {
            Console.WriteLine("Создаем бота");
            var httpCliendHandler = new HttpClientHandler() { Proxy = ProxyParser.ProxyList[0] };
            HttpClient hc = new HttpClient(httpCliendHandler);
            TelegramBot = new TelegramBotClient(getToken(),hc);
        }

        /// <summary>
        /// Задание настроек боту без прокси
        /// </summary>
        public static void setBot()
        {
            TelegramBot = new TelegramBotClient(getToken());
        }
        /// <summary>
        /// Получение значения токен ключа
        /// </summary>
        /// <returns>токен</returns>
        private static string getToken()
        {
            using (StreamReader sr = new StreamReader("token.txt"))
            {
               return sr.ReadLine();
            }
 
        }
    }
}
