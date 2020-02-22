using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.Enums;
using Newtonsoft.Json;
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
                if (u)
                {
                    ProxyParser.SaveCurrentProxy();
                }


                Bot.TelegramBot.OnMessage += MessageParser;

                //Bot.TelegramBot.OnMessage += FileSeaking;
                //Bot.TelegramBot.OnMessage += PhotoSeaking;

                //Bot.TelegramBot.OnCallbackQuery += BotOnCallbackQueryReceived;

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


        //static async void BotOnCallbackQueryReceived(object sender, Telegram.Bot.Args.CallbackQueryEventArgs e)
        //{
        //    var temp =Convert.ToInt32 (e.CallbackQuery.Data);
        //    var e1 = e.CallbackQuery.Message;
        //    Console.WriteLine(e1.MessageId);
        //    await Bot.TelegramBot.AnswerCallbackQueryAsync(e.CallbackQuery.Id, "тест");


        //    var inlineKeyboard = new InlineKeyboardMarkup(new[]
        //                        {
        //                new [] // first row
        //                {
        //                    InlineKeyboardButton.WithCallbackData("Сильное сжатие", "0"),
        //                    InlineKeyboardButton.WithCallbackData("среднее сжатие", "1"),
        //                    InlineKeyboardButton.WithCallbackData("Слабое сжатие", "2")
        //                }
        //            });

        //    Bot.TelegramBot.SendTextMessageAsync(e.Message.Chat.Id, "Выберите вариант сжатия", replyMarkup: inlineKeyboard);

        //}



        static void MessageParser(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            switch (e.Message.Type)
            {
                case MessageType.Text:
                    MessageSeaker(e);
                    break;
                case MessageType.Photo:
                    PhotoSeaking(e);
                    break;
                case MessageType.Document:
                    FileSeaking(e);
                    break;
                case MessageType.Audio:
                    AudioSeaking(e);
                    break;
                case MessageType.Sticker:
                    StikerSeaking(e);
                    break;
                case MessageType.Location:
                    LocationSeaker(e);
                    break;
                case MessageType.Contact:
                    break;
                case MessageType.Voice:
                    break;
            }

        }

        static void LocationSeaker(Telegram.Bot.Args.MessageEventArgs e)
        {
            Console.WriteLine($"{e.Message.Location.Latitude} - {e.Message.Location.Longitude}");
            var loc = (e.Message.Location.Latitude, e.Message.Location.Longitude);
            SaveSerializeFile(JsonConvert.SerializeObject(loc),
                $"{ e.Message.MessageId}",
                e.Message.Chat.Id,
                e.Message.Type);

        }

        static void SaveSerializeFile(string json,string path,long id,MessageType type)
        {
            string fullpath = id + @"\" + type + @"\" ;
            FileInfo fi = new FileInfo(fullpath+path);
            createrFile(id, "", path, type);

            using (StreamWriter sw = fi.CreateText())
            {
                sw.WriteLine(json);
            }
        } 

        static async void MessageSeaker(Telegram.Bot.Args.MessageEventArgs e)
        {
            string text = $"{DateTime.Now.ToLongTimeString()}: {e.Message.Chat.FirstName} {e.Message.Chat.Id} {e.Message.Text}";
            await Bot.TelegramBot.SendTextMessageAsync(e.Message.Chat.Id, "Дратути");
            Console.WriteLine($"{e.Message.Chat.Username}:{e.Message.Text}");
            var user = new TelegramUser(e.Message.Chat.Id, e.Message.Chat.Username);
        }

        static void AudioSeaking(Telegram.Bot.Args.MessageEventArgs e)
        {
            Console.WriteLine($"Audio FileID:{e.Message.Audio.FileId}");
            Console.WriteLine($"Audio Title:{e.Message.Audio.Title}");
            DownLoad(e.Message.Audio.FileId,
                   e.Message.Audio.FileUniqueId,
                   e.Message.Chat.Username == null ? e.Message.Chat.FirstName : e.Message.Chat.Username,
                   e.Message.Chat.Id,
                   e.Message.Type);
             
        }

        static void StikerSeaking(Telegram.Bot.Args.MessageEventArgs e)
        {
            Console.WriteLine($"Stiker FileID:{e.Message.Sticker.FileId}");
            Console.WriteLine($"Stiker Emoji:{e.Message.Sticker.Emoji}");
            DownLoad(e.Message.Sticker.FileId,
                   e.Message.Sticker.Emoji,
                   e.Message.Chat.Username == null ? e.Message.Chat.FirstName : e.Message.Chat.Username,
                   e.Message.Chat.Id,
                   e.Message.Type);
        }

        static  void FileSeaking(Telegram.Bot.Args.MessageEventArgs e)
        {
                Console.WriteLine(e.Message.Document.FileId);
                Console.WriteLine(e.Message.Document.FileName);
                Console.WriteLine(e.Message.Document.FileSize);

                DownLoad(e.Message.Document.FileId,
                    e.Message.Document.FileName,
                    e.Message.Chat.Username == null ? e.Message.Chat.FirstName : e.Message.Chat.Username,
                    e.Message.Chat.Id,
                    e.Message.Type);

        }


        static async void PhotoSeaking (Telegram.Bot.Args.MessageEventArgs e)
        {
            Console.WriteLine(e.Message.Photo.Length);
            Console.WriteLine(e.Message.MessageId);
            var type = e.Message.Type;
            var photo = e.Message.Photo;

            await Bot.TelegramBot.SendPhotoAsync(e.Message.Chat.Id, photo[2].FileId);

            foreach (var item in photo)
            {
                Console.WriteLine($"{item.FileId} {item.FileUniqueId} {item.FileSize} ");
            }
            DownLoad(photo[2].FileId,
               "ф" + photo[2].FileSize + ".jpg",
               e.Message.Chat.Username == null ? e.Message.Chat.FirstName : e.Message.Chat.Username,
               e.Message.Chat.Id,
               type);
        }




        static async void DownLoad(string field, string path, string name,long id,Telegram.Bot.Types.Enums.MessageType type)
        {
            
                var file = await Bot.TelegramBot.GetFileAsync(field);
                createrFile(id, name, path,type);
                // var user = new TelegramUser(id, name);
                //UsersBase.putUsersFile(user, id + @"\" + path);
                try
                {
                    using (BufferedStream bs = new BufferedStream(new FileStream(id + @"\" +type+ @"\" + path, FileMode.Create)))
                        await Bot.TelegramBot.DownloadFileAsync(file.FilePath, bs);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                 DownLoad(field, "_"+path , name, id,type);
                    return;
                }
                Console.WriteLine($"Докачал файл {path}");

        }


        static async void Send(string path,long id,Telegram.Bot.Types.Enums.MessageType type)
        {
            switch (type)
            {
                case MessageType.Audio:
                    using (BufferedStream bs2 = new BufferedStream(File.OpenRead(id + @"\" + type + @"\" + "AgADSAUAAuROiEo")))
                    {
                        InputOnlineFile iof = new InputOnlineFile(bs2, new FileInfo(id + @"\" + type + @"\" + "AgADSAUAAuROiEo").Name);
                        await Bot.TelegramBot.SendAudioAsync(
                            chatId: id,
                            audio: iof,
                            caption: "Ваш файл"
                            );
                    }
                    break;
                case MessageType.Document:
                using (BufferedStream bs2 = new BufferedStream(File.OpenRead(id + @"\" + type + @"\" + path)))
                {
                    InputOnlineFile iof = new InputOnlineFile(bs2, new FileInfo(id + @"\" + type + @"\" + path).Name);
                    await Bot.TelegramBot.SendDocumentAsync(
                        chatId: id,
                        document: iof,
                        caption: "Ваш файл"
                        );
                 }
                    break;
        }
            
        }

        static void createrFile(long id ,string name, string path,Telegram.Bot.Types.Enums.MessageType type)
        {
            FileInfo fi = new FileInfo(id + @"\" + type + @"\" + path);

            if (!Directory.Exists(id + @"\" + type + @"\" + path))
            {
                Directory.CreateDirectory(fi.DirectoryName);
            }
        }
    }
}
