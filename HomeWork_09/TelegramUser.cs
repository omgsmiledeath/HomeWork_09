using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork_09
{
    class TelegramUser : IEquatable <TelegramUser>
    {
        /// <summary>
        /// Коснтруктор класса пользователь телеграмм
        /// </summary>
        /// <param name="id">Id пользователя</param>
        /// <param name="name">Имя иди Nick</param>
        public TelegramUser (long id,string name)
        {
            this.id = id;
            this.name = name;
            messages = new List<string>();

        }
        private long id;
        /// <summary>
        /// Свойство по получению и установке значения ID для пользователя
        /// </summary>
        public long Id
        {
            get { return this.id; }
            set { id = value; }
        }

        private string name;
        /// <summary>
        /// Свойство по получению и установке значения Name для пользователя
        /// </summary>
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }



        /// <summary>
        /// Cписок сообщений пользователя
        /// </summary>
        public List<string> messages;

        /// <summary>
        /// Добавление сообщения в список
        /// </summary>
        /// <param name="msg"></param>
        public void addMessage(string msg) => messages.Add(msg);



        /// <summary>
        /// Метод для сравнения пользователей
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool Equals(TelegramUser user) => user.Id == this.id;


    }
}
