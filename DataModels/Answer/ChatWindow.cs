using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.Answer
{
    public class ChatWindow
    {
        public ChatWindow()
        {
            this.ChatList = new List<ChatItem>();
            return;
        }

        public List<ChatItem> ChatList;
    }
}
