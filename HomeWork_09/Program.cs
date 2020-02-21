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

            //var proxy = new HttpToSocks5Proxy($"96.44.133.110", 58690); 
            //var proxy = new WebProxy()
            //{
            //    Address = new Uri($"http://45.177.16.129:999"),
            //    UseDefaultCredentials = false,
            //};

            ////var proxy = new HttpToSocks5Proxy(new[] {
            ////new ProxyInfo("tor-proxy.com", 1080),
            ////new ProxyInfo("random-socks.com", 1090),
            ////new ProxyInfo("tor-proxy.com", 1080)
            ////});

            ////proxy.ResolveHostnamesLocally = true;
            //var httpCliendHandler = new HttpClientHandler() { Proxy = proxy };
            //HttpClient hc = new HttpClient(httpCliendHandler);
            Bot.Start();
            UsersBase.saveBase();
            
        }
        


        

        
    }

         
    }

