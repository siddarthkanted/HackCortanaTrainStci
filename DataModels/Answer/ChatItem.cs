using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.Answer
{
    public class ChatItem
    {
        public ChatItem()
        {
            this.Id = string.Empty;
            this.ChatText = string.Empty;
        }

        public string Id;

        public string ChatText;

        public ChatListType ListType;

        public List<string> ChatList;
    }

    public enum ChatListType
    {
        None,
        Row,
        Column
    }
}
