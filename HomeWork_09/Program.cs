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
       
        
        public static void Main(string[] args)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            bool flag1 = false;
            bool flag2 = false;
            while (true)
            {
                if (flag1 & flag2) break;
                if (!File.Exists("token.txt"))
                {
                    Console.Write("Не обнаржуен файл с токен ключем\n Введите ваш токен и файл будет автоматически создан:");
                    string token = Console.ReadLine();
                    using (StreamWriter sw = new StreamWriter("token.txt"))
                    {
                        sw.WriteLine(token);
                    }
                }
                else flag1 = true;

                if (!File.Exists("proxy.txt")&(flag2==false))
                {
                    Console.WriteLine("Хотите указать вашу прокси? в противном случае будет использована функция автопоиска прокси(Y - да : N - нет)");
                    while (true)
                    {
                        var key = Console.ReadKey(true);
                        if (key.Key == ConsoleKey.Y)
                        {
                            Console.Write("Хорошо, теперь укажите адресс и порт для вашей прокси \n Адресс:");
                            string host = Console.ReadLine();
                            Console.Write("Порт:");
                            int port = 0;
                            Int32.TryParse(Console.ReadLine(), out port);
                            WebProxy wb = new WebProxy(host, port);
                            string json = JsonConvert.SerializeObject(wb);
                            File.WriteAllText("proxy.txt", json);
                            break;
                        }
                        if(key.Key == ConsoleKey.N)
                        {
                            flag2 = true;
                            break;
                        }
                    }
                }
                else flag2 = true;

            }
            
                       

            

            Bot.Start();
            UsersBase.saveBase();
            
        }
        


        

        
    }

         
    }

