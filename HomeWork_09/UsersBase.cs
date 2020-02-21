using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
namespace HomeWork_09
{
    static class UsersBase
    {
        static List<TelegramUser> users;

        static public void getUsersFromFile(string path ="users.txt")
        {
            if (!File.Exists(path)) File.Create(path);
            string json1 = File.ReadAllText(path);
            users = JsonConvert.DeserializeObject<List<TelegramUser>>(json1);
            if (users == null) users = new List<TelegramUser>();
        }

        static public void putUsersMessage (TelegramUser user, Telegram.Bot.Args.MessageEventArgs e)
        {
            

            if (!users.Contains(user)) users.Add(user);

            users[users.IndexOf(user)].addMessage($"{e.Message.Text}");
            saveBase();
        }

        static public void putUsersFile(TelegramUser user,string path)
        {
            if (!users.Contains(user)) users.Add(user);
            users[users.IndexOf(user)].addFile($"{path}");
            saveBase();
        }

        static public void saveBase()
        {
            string json = JsonConvert.SerializeObject(users);
            File.WriteAllText("users.txt", json);
        }
    }
}
