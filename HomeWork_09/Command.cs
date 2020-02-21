using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Telegram.Bot.Types.InputFiles;

namespace HomeWork_09
{
    public static class Command
    {

        public static void Test()
        {
            try
            {
                Console.WriteLine("Пытаюсь подключится");
                var u = Bot.TelegramBot.TestApiAsync().Result;
                Console.WriteLine(u);
                if(u)
                {
                    ProxyParser.SaveCurrentProxy();
                }


                Bot.TelegramBot.OnMessage += HanldeMessage;
                Bot.TelegramBot.StartReceiving();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ProxyParser.BadProxyRemove();
                Bot.setBotWithProxy();
                Test();
                return;
            }
            
        }

        static void HanldeMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            string text = $"{DateTime.Now.ToLongTimeString()}: {e.Message.Chat.FirstName} {e.Message.Chat.Id} {e.Message.Text}";
            Bot.TelegramBot.SendTextMessageAsync(e.Message.Chat.Id, "Дратути");
            Console.WriteLine($"{e.Message.Chat.Username}:{e.Message.Text}");
            var user = new TelegramUser(e.Message.Chat.Id, e.Message.Chat.Username);
            UsersBase.putUsersMessage(user,e);
            if (e.Message.Type == Telegram.Bot.Types.Enums.MessageType.Document)
            {
                Console.WriteLine(e.Message.Document.FileId);
                Console.WriteLine(e.Message.Document.FileName);
                Console.WriteLine(e.Message.Document.FileSize);

                DownLoad(e.Message.Document.FileId,
                    e.Message.Document.FileName,
                    e.Message.Chat.Username == null ? e.Message.Chat.FirstName : e.Message.Chat.Username,
                    e.Message.Chat.Id);
            }
            
        }

        static async void DownLoad(string field, string path, string name,long id)
        {
            FileStream fs = null;
            var file = await Bot.TelegramBot.GetFileAsync(field);
            FileInfo fi = new FileInfo(id + @"\" + path);
            
            if (!Directory.Exists(name + @"\" + path))
            {
                Directory.CreateDirectory(fi.DirectoryName);
            }
               // fs = new FileStream(id + @"\" + path, FileMode.Create);
            
            var user = new TelegramUser(id, name);
            UsersBase.putUsersFile(user, id + @"\" + path);
            using (BufferedStream bs = new BufferedStream(new FileStream(id + @"\" + path, FileMode.Create)))
            await Bot.TelegramBot.DownloadFileAsync(file.FilePath, bs);
            using (BufferedStream bs2 =  new BufferedStream(File.OpenRead(id + @"\" + path))) {
                InputOnlineFile iof = new InputOnlineFile(bs2, new FileInfo(id + @"\" + path).Name);
                await Bot.TelegramBot.SendDocumentAsync(
                    chatId: id,
                    document: iof,
                    caption: "Ваш файл"
                    );
            }

        }


        static async void Send(string path,long id)
        {
            await Bot.TelegramBot.SendDocumentAsync(id, path);
        }

    }
}
