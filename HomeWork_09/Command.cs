using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.Enums;
using Newtonsoft.Json;
using System.Threading;

namespace HomeWork_09
{
    public static class Command
    {
        /// <summary>
        /// Запуск команд бота
        /// </summary>
        public static void Start()
        {
            try
            {
                Console.WriteLine("~~Пытаюсь подключится~~");
                var u = Bot.TelegramBot.TestApiAsync().Result;
               // Console.WriteLine(u);
                if (u)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Соединение установленно");
                    Console.ForegroundColor = ConsoleColor.White;
                    ProxyParser.SaveCurrentProxy();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Введен не верный токен для телеграм бота");
                    Console.ForegroundColor = ConsoleColor.White;
                }

                Bot.TelegramBot.OnMessage += MessageParser;
                Bot.TelegramBot.OnCallbackQuery += TypeOfFile;

                Bot.TelegramBot.StartReceiving();
                Console.ReadKey();
                Bot.TelegramBot.StopReceiving();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Попытка соединения не удалась , меняем прокси");
                Console.ForegroundColor = ConsoleColor.White;
                ProxyParser.BadProxyRemove();
                Bot.setBotWithProxy();
                Start();
                return;
            }

        }

        /// <summary>
        /// Метод который ожидает в сообщении ответ выбора с клавиатуры , указанного типа файла
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static async void TypeOfFile(object sender, Telegram.Bot.Args.CallbackQueryEventArgs e)
        {
           Console.WriteLine(e.CallbackQuery.Message.MessageId);
            await Bot.TelegramBot.EditMessageTextAsync(e.CallbackQuery.Message.Chat.Id,e.CallbackQuery.Message.MessageId,$"Выбор сделан");
            var mess = e.CallbackQuery.Data;
            Console.WriteLine(mess);
            switch (mess)
            {
                case "Document":
                        DirectoryInfo di = new DirectoryInfo(e.CallbackQuery.Message.Chat.Id + @"\" + "Document");
                    if (di.Exists)
                    {
                        var files = di.GetFiles();
                        await Bot.TelegramBot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id,
                            "Выбрана отправка документов");
                        await Bot.TelegramBot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, "Вам необходимо отправить полностью название " +
                            "файла укзанное в < >", replyMarkup: new ReplyKeyboardRemove());
                        foreach (var file in files)
                        {
                            await Bot.TelegramBot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, $"Имя файла - <{file.Name}>: \n" +
                                $"Размер в КБ - {(float)file.Length / 1_024}");
                        }
                        Bot.TelegramBot.OnMessage -= MessageParser;
                        Bot.TelegramBot.OnMessage += DocumentSender;
                    }
                    else
                    {
                        await Bot.TelegramBot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id,
                            "Сохраненных документов не обнаруженно");

                    }
                        break;

                    case "Audio":
                    
                        di = new DirectoryInfo(e.CallbackQuery.Message.Chat.Id + @"\" + "Audio");
                    if (di.Exists)
                    {
                        var files = di.GetFiles();
                        await Bot.TelegramBot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, "Выбрана отправка аудио");
                        await Bot.TelegramBot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, "Вам необходимо отправить полностью название " +
                            "файла укзанное в < >");
                        foreach (var file in files)
                        {
                            await Bot.TelegramBot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, $"Имя файла - <{file.Name}>: \n" +
                                $"Размер в КБ - {(float)file.Length / 1_024}");
                        }
                        Bot.TelegramBot.OnMessage -= MessageParser;
                        Bot.TelegramBot.OnMessage += AudioSender;
                    }
                    else
                    {
                        await Bot.TelegramBot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id,
                            "Сохраненных аудио не обнаруженно");
                    }
                    break;

                    case "Sticker":

                        di = new DirectoryInfo(e.CallbackQuery.Message.Chat.Id + @"\" + "Sticker");
                    if (di.Exists)
                    {
                        var files = di.GetFiles();
                        await Bot.TelegramBot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, "Выбрана отправка стикеров");
                        await Bot.TelegramBot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, "Вам необходимо отправить полностью название " +
                            "файла укзанное в < >");
                        foreach (var file in files)
                        {
                            await Bot.TelegramBot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, $"Имя файла - <{file.Name}>: \n" +
                                $"Размер в КБ - {(float)file.Length / 1_024}");
                        }
                        Bot.TelegramBot.OnMessage -= MessageParser;
                        Bot.TelegramBot.OnMessage += StickerSender;
                    }
                    else
                    {
                        await Bot.TelegramBot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id,
                            "Сохраненных стикеров не обнаруженно");
                    }
                    break;

                    case "Location":
                    await Bot.TelegramBot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, "Выбрана отправка данных локаций");
                    di = new DirectoryInfo(e.CallbackQuery.Message.Chat.Id + @"\" + "Location");
                    if (di.Exists)
                    {
                        var files = di.GetFiles();
                        foreach (var file in files)
                        {
                            var loc = DeserializeLocation(file.FullName);
                            await Bot.TelegramBot.SendLocationAsync(e.CallbackQuery.Message.Chat.Id, loc.latitude, loc.laptitude);
                        }
                    }
                    else
                    {
                        await Bot.TelegramBot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id,
                            "Сохраненных локаций не обнаруженно");
                    }
                    break;
                    case "Photo":
                        di = new DirectoryInfo(e.CallbackQuery.Message.Chat.Id + @"\" + "Photo");
                    if (di.Exists)
                    {
                        var files = di.GetFiles();
                        await Bot.TelegramBot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, "Выбрана отправка фото");
                        await Bot.TelegramBot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, "Вам необходимо отправить полностью название " +
                            "файла укзанное в < >");
                        foreach (var file in files)
                        {
                            await Bot.TelegramBot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, $"Имя файла - <{file.Name}>: \n" +
                                $"Размер в КБ - {(float)file.Length / 1_024}");
                        }
                        Bot.TelegramBot.OnMessage -= MessageParser;
                        Bot.TelegramBot.OnMessage += PhotoSender;
                    }
                    else
                    {
                        await Bot.TelegramBot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id,
                            "Сохраненных фото не обнаруженно");
                    }
                    break;
                    case "Voice":
                        di = new DirectoryInfo(e.CallbackQuery.Message.Chat.Id + @"\" + "Voice");
                    if (di.Exists)
                    {
                       var files = di.GetFiles();
                        await Bot.TelegramBot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, "Выбрана отправка голосового файла");
                        await Bot.TelegramBot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, "Вам необходимо отправить полностью название " +
                            "файла укзанное в < >");
                        foreach (var file in files)
                        {
                            await Bot.TelegramBot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, $"Имя файла - <{file.Name}>: \n" +
                                $"Размер в КБ - {(float)file.Length / 1_024}");
                        }
                        Bot.TelegramBot.OnMessage -= MessageParser;
                        Bot.TelegramBot.OnMessage += VoiceSender;
                    }
                    else
                    {
                        await Bot.TelegramBot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id,
                            "Сохраненных голосовых сообщений не обнаруженно");
                    }
                    break;
            }
  
        }
        /// <summary>
        /// ДЕсериализация файла с данными локации
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        static (float latitude,float laptitude) DeserializeLocation(string path)
        {
            string json = String.Empty;
            using (StreamReader sr = new StreamReader(path))
            {
                json = sr.ReadToEnd();
            }
            return JsonConvert.DeserializeObject<ValueTuple<float, float>>(json);
        }

        /// <summary>
        /// Метод который ожидает название  файла в сообщении и отправляет указанный файл пользователю
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static async void DocumentSender (object sender,Telegram.Bot.Args.MessageEventArgs e)
        {
            var mess = e.Message.Text;
            string path = CreaterPath(e.Message.Chat.Id, mess, MessageType.Document);
            UsersBase.Saver(e);
            if (File.Exists(path))
            {
                Send(path, e.Message.Chat.Id, MessageType.Document);
            Bot.TelegramBot.OnMessage -= DocumentSender;
            Bot.TelegramBot.OnMessage += MessageParser;
            }
            else await Bot.TelegramBot.SendTextMessageAsync(e.Message.Chat.Id, "Вы указали не верное имя файла, повторите ввод");
        }

        /// <summary>
        /// Метод который ожидает название аудио файла в сообщении и отправляет указанный файл пользователю
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static async void AudioSender(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            var mess = e.Message.Text;
            string path = CreaterPath(e.Message.Chat.Id, mess, MessageType.Audio);
            UsersBase.Saver(e);
            if (File.Exists(path))
            {
                Send(path, e.Message.Chat.Id, MessageType.Audio);
                Bot.TelegramBot.OnMessage -= AudioSender;
                Bot.TelegramBot.OnMessage += MessageParser;
             }
            else await Bot.TelegramBot.SendTextMessageAsync(e.Message.Chat.Id, "Вы указали не верное имя файла, повторите ввод");
    }

        /// <summary>
        /// Метод который ожидает название стикера в сообщении и отправляет указанный файл пользователю
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static async void StickerSender(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            var mess = e.Message.Text;
            string path = CreaterPath(e.Message.Chat.Id, mess, MessageType.Sticker);
            UsersBase.Saver(e);
            if (File.Exists(path))
            {
                Send(path, e.Message.Chat.Id, MessageType.Sticker);
                Bot.TelegramBot.OnMessage -= StickerSender;
                Bot.TelegramBot.OnMessage += MessageParser;
            }
            else await Bot.TelegramBot.SendTextMessageAsync(e.Message.Chat.Id, "Вы указали не верное имя файла, повторите ввод");
        }

        /// <summary>
        /// Метод который ожидает название голосового файла в сообщении и отправляет указанный файл пользователю
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static async void VoiceSender(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            var mess = e.Message.Text;
            string path = CreaterPath(e.Message.Chat.Id, mess, MessageType.Voice);
            UsersBase.Saver(e);
            if (File.Exists(path))
            {
                Send(path, e.Message.Chat.Id, MessageType.Voice);
                Bot.TelegramBot.OnMessage -= VoiceSender;
                Bot.TelegramBot.OnMessage += MessageParser;
            }
            else await Bot.TelegramBot.SendTextMessageAsync(e.Message.Chat.Id, "Вы указали не верное имя файла, повторите ввод");
        }

        /// <summary>
        /// Метод который ожидает название файла в сообщении и отправляет указанный файл пользователю
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static async void PhotoSender(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            var mess = e.Message.Text;
            string path = CreaterPath(e.Message.Chat.Id, mess, MessageType.Photo);
            UsersBase.Saver(e);
            if (File.Exists(path))
            {
                Send(path, e.Message.Chat.Id, MessageType.Photo);
                Bot.TelegramBot.OnMessage -= PhotoSender;
                Bot.TelegramBot.OnMessage += MessageParser;
            }
            else await Bot.TelegramBot.SendTextMessageAsync(e.Message.Chat.Id, "Вы указали не верное имя файла, повторите ввод");
            
        }


        /// <summary>
        /// Метод для отправки клавиатуры выбора типа файлов для отправки пользователю
        /// </summary>
        /// <param name="id">ID пользователя в телеграмме</param>
        static async void GetType(long id)
        {
            var inlineKeyboard = new InlineKeyboardMarkup(new[]
                                {
                        new []
                        {
                            InlineKeyboardButton.WithCallbackData("Document", $"{MessageType.Document}"),
                            InlineKeyboardButton.WithCallbackData("Photo", $"{MessageType.Photo}"),
                            InlineKeyboardButton.WithCallbackData("Audio", $"{MessageType.Audio}"),
                            InlineKeyboardButton.WithCallbackData("Stiker", $"{MessageType.Sticker}"),
                            InlineKeyboardButton.WithCallbackData("Location", $"{MessageType.Location}"),
                            InlineKeyboardButton.WithCallbackData("Voice", $"{MessageType.Voice}")
                        }
                    });
            try
            {
                await Bot.TelegramBot.SendTextMessageAsync(id, "Выберите нужный тип файла:", replyMarkup: inlineKeyboard);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                GetType(id);
                return;
            }
            
        }

        /// <summary>
        /// Метод вызывающий конкретный обработчик для сообщения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">сообщений из телеграма</param>
        static async void MessageParser(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            if(e.Message.Text=="/start")
            {
                await Bot.TelegramBot.SendTextMessageAsync(e.Message.Chat.Id,
                    $"Добро пожаловать в бот файловое облако. Для описания бота /info ");
                return;
            }
            if (e.Message.Text=="/info")
            {
                await Bot.TelegramBot.SendTextMessageAsync(e.Message.Chat.Id, 
                    $"Вы можете отправлять : файлы , фото , аудио файлы, данные локации,голосовые сообщения и стикеры. Чтоб получить файлы обратно напишите команду /export ");
            }

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
                case MessageType.Voice:
                    VoiceSeaker(e);
                    break;
            }

        }

        /// <summary>
        /// Метод обрабатывающий сообщение из телеграма на предмет голосовых сообщений
        /// </summary>
        /// <param name="e">входящее сообщение из телеграма</param>
        static void VoiceSeaker(Telegram.Bot.Args.MessageEventArgs e)
        {
            Console.WriteLine(e.Message.Voice.FileUniqueId);
            string path = CreaterPath(e.Message.Chat.Id, e.Message.Voice.FileUniqueId, e.Message.Type);
            DownLoad(e.Message.Voice.FileId, path);  
        }


        /// <summary>
        /// Метод обрабатывающий сообщение из телеграма на предмет данных локации
        /// </summary>
        /// <param name="e">входящее сообщение из телеграма</param>
        static void LocationSeaker(Telegram.Bot.Args.MessageEventArgs e)
        {
            Console.WriteLine($"{e.Message.Location.Latitude} - {e.Message.Location.Longitude}");
            var loc = (Latitude:e.Message.Location.Latitude, Longitude: e.Message.Location.Longitude);
            SaveSerializeFile(JsonConvert.SerializeObject(loc),
                $"{ e.Message.MessageId}",
                e.Message.Chat.Id,
                e.Message.Type);
        }

        /// <summary>
        /// Метод для сохранения сериализованных не типичных данных
        /// </summary>
        /// <param name="json">Сериализованные в JSON данные</param>
        /// <param name="path">название файла</param>
        /// <param name="id">id пользователя</param>
        /// <param name="type">тип данных для сохранения в отдельной папке</param>
        static void SaveSerializeFile(string json,string path,long id,MessageType type)
        {
            string fullpath = id + @"\" + type + @"\" ;
            FileInfo fi = new FileInfo(fullpath+path);
            createrFile(id, path, type);
            using (StreamWriter sw = fi.CreateText())
            {
                sw.WriteLine(json);
            }
        }

        /// <summary>
        /// Метод обрабатывающий сообщение из телеграма
        /// </summary>
        /// <param name="e">входящее сообщение из телеграма</param>
        static void MessageSeaker(Telegram.Bot.Args.MessageEventArgs e)
        {
            string text = $"{DateTime.Now.ToLongTimeString()}: {e.Message.Chat.FirstName} {e.Message.Chat.Id} {e.Message.Text}";
            Console.WriteLine($"{e.Message.Chat.Username}:{e.Message.Text}");
            var user = new TelegramUser(e.Message.Chat.Id, e.Message.Chat.Username);
            UsersBase.putUsersMessage(user, e);
            if (e.Message.Text == @"/export")
            {

                GetType(e.Message.Chat.Id);
            }

        }


        /// <summary>
        /// Метод обрабатывающий сообщение из телеграма на предмет аудио файлов
        /// </summary>
        /// <param name="e">входящее сообщение из телеграма</param>
        static async void AudioSeaking(Telegram.Bot.Args.MessageEventArgs e)
        {

            string path = CreaterPath(e.Message.Chat.Id, e.Message.Audio.FileUniqueId, e.Message.Type);
            DownLoad(e.Message.Audio.FileId,
                   path);
            await Bot.TelegramBot.SendTextMessageAsync(e.Message.Chat.Id, "Ваш аудио файл принят");
             
        }
        /// <summary>
        /// Метод обрабатывающий сообщение из телеграма стикеров
        /// </summary>
        /// <param name="e">входящее сообщение из телеграма</param>
        static async void StikerSeaking(Telegram.Bot.Args.MessageEventArgs e)
        {
            
            string path = CreaterPath(e.Message.Chat.Id, e.Message.Sticker.FileUniqueId+".webp", e.Message.Type);
            DownLoad(e.Message.Sticker.FileId,
                   path);
            await Bot.TelegramBot.SendTextMessageAsync(e.Message.Chat.Id, "Ваш стикер принят");
        }
        /// <summary>
        /// Метод обрабатывающий сообщение из телеграма на предмет файлов
        /// </summary>
        /// <param name="e">входящее сообщение из телеграма</param>
        static async void FileSeaking(Telegram.Bot.Args.MessageEventArgs e)
        {

            string path = CreaterPath(e.Message.Chat.Id, e.Message.Document.FileName, e.Message.Type);
                DownLoad(e.Message.Document.FileId,
                  path);
            await Bot.TelegramBot.SendTextMessageAsync(e.Message.Chat.Id, "Ваш файл принят");
        }

        /// <summary>
        /// Метод обрабатывающий сообщение из телеграма на предмет фото
        /// </summary>
        /// <param name="e">входящее сообщение из телеграма</param>
        static async void PhotoSeaking (Telegram.Bot.Args.MessageEventArgs e)
        {
            var type = e.Message.Type;
            var photo = e.Message.Photo;

            DownLoad(photo[2].FileId,
              CreaterPath(e.Message.Chat.Id,
              "ф" + photo[2].FileSize + ".jpg",
              type));
            await Bot.TelegramBot.SendTextMessageAsync(e.Message.Chat.Id, "Ваше фото принято");
        }



        /// <summary>
        /// Метод закгружающий файлы 
        /// </summary>
        /// <param name="field">Уникальный id файла</param>
        /// <param name="path">конечный путь куда скачается файл</param>
        static async void DownLoad(string field, string path)
        {
            
                var file = await Bot.TelegramBot.GetFileAsync(field);

                try
                {
                    using (BufferedStream bs = new BufferedStream(new FileStream(path, FileMode.Create)))
                        await Bot.TelegramBot.DownloadFileAsync(file.FilePath, bs);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                 DownLoad(field, "_"+path );
                    return;
                }
                Console.WriteLine($"Докачал файл {path}");

        }

        /// <summary>
        /// Метод отправки файлов и тд , пользователю
        /// </summary>
        /// <param name="path">путь до файла</param>
        /// <param name="id">ID телеграм пользователя</param>
        /// <param name="type">тип файла</param>
        static async void Send(string path, long id, Telegram.Bot.Types.Enums.MessageType type)
        {

            await Bot.TelegramBot.SendTextMessageAsync(id, "Начата отправка файла , процесс может занять какое то время в зависимости от объема файла.");
            
            using (BufferedStream bs2 = new BufferedStream(File.OpenRead(path)))
            {
                InputOnlineFile iof = new InputOnlineFile(bs2, new FileInfo(path).Name);
                switch (type)
                {
                    case MessageType.Audio:
                        await Bot.TelegramBot.SendAudioAsync(
                            chatId: id,
                            audio: iof,
                            caption: "Ваш файл"
                            );
                        break;
                    case MessageType.Voice:
                        await Bot.TelegramBot.SendVoiceAsync(
                            chatId: id,
                            voice: iof,
                            caption: "Ваш файл"
                            );
                        break;
                    case MessageType.Document:
                        await Bot.TelegramBot.SendDocumentAsync(
                            chatId: id,
                            document: iof,
                            caption: "Ваш файл"
                            );
                        break;
                    case MessageType.Photo:
                        await Bot.TelegramBot.SendPhotoAsync(
                           chatId: id,
                           photo: iof,
                           caption: "Ваш файл"
                           );
                        break;
                    case MessageType.Sticker:
                        await Bot.TelegramBot.SendStickerAsync(
                           chatId: id,
                           sticker: iof
                           );
                        break;
                }

            }
        }
        /// <summary>
        /// Метод для создания файлов в папках у разных пользователей сортированные по типу отправляемых файлов
        /// </summary>
        /// <param name="id">ID пользователя телеграм</param>
        /// <param name="path">путь до файла</param>
        /// <param name="type">тип файла</param>
        static void createrFile(long id , string path,Telegram.Bot.Types.Enums.MessageType type)
        {
            FileInfo fi = new FileInfo(id + @"\" + type + @"\" + path);

            if (!Directory.Exists(id + @"\" + type + @"\" + path))
            {
                Directory.CreateDirectory(fi.DirectoryName);
            }
        }
        /// <summary>
        /// Метод для создания пути для файлов 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="path"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        static string CreaterPath(long id, string path, Telegram.Bot.Types.Enums.MessageType type)
        {
            
            string result = id + @"\" + type + @"\" + path;
            path = path.Trim(new char[] { ' ', '<', '>' });
            Console.WriteLine(result);
            result = result.Trim(new char[] { ' ', '<', '>' });
            Console.WriteLine(result);
            if (!File.Exists(path)) createrFile(id, path, type);
            return result;
        }
    }
}
