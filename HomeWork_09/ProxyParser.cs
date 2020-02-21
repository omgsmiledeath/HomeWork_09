using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
namespace HomeWork_09
{
    static class ProxyParser
    {
        static private List<WebProxy> proxylist = new List<WebProxy>();
        static public List<WebProxy> ProxyList { get { return proxylist; } }
        static public void getProxyList()
        {

            using (WebClient wc = new WebClient())
            {
                string htmp = wc.DownloadString($"https://sslproxies.org/");
                Regex regex = new Regex(@"\d+(.)\d+(.)\d+(.)\d+(<\/td><td>)\d+");
                MatchCollection mc = regex.Matches(htmp);
                if (mc.Count > 0)
                {
                    Console.WriteLine("Начал парсить");
                    foreach (Match e in mc)
                    {
                        string[] temp = e.Value.Split(new string[] { @"</td><td>" }, StringSplitOptions.None);
                        proxylist.Add(new WebProxy(temp[0], int.Parse(temp[1])));
                    }
                }
            };
            Console.WriteLine("Прокси получены");
        }

        static public void BadProxyRemove()
        {
            if(proxylist.Count==1)
            {
                getProxyList();
            }
            else proxylist.RemoveAt(0);
        }

        static public void SaveCurrentProxy()
        {
            string json =JsonConvert.SerializeObject(proxylist[0]);
            File.WriteAllText("proxy.txt", json);
            
        }

        static public void LoadProxy()
        {
            if (File.Exists("proxy.txt"))
            {
                string json = File.ReadAllText("proxy.txt");
                proxylist.Add(JsonConvert.DeserializeObject<WebProxy>(json));
                Console.WriteLine("Загружена сохраненная прокся");
            }
            
            else getProxyList();
        }
    }

}
