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
        public TelegramUser (long id,string name)
        {
            this.id = id;
            this.name = name;
            messages = new List<string>();
            Files = new List<string>();
        }
        private long id;

        public long Id
        {
            get { return this.id; }
            set { id = value; }
        }

        private string name;

        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        public List<string> Files;


        public List<string> messages;

        public void addMessage(string msg) => messages.Add(msg);
  

        public void addFile(string path) => Files.Add(path);
            

        public bool Equals(TelegramUser user) => user.Id == this.id;


    }
}
