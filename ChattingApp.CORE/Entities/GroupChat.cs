using ChattingApp.CORE.DTO.GroupChatDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Entities
{
    public class GroupChat:Chat
    {
        public string GroupName { get; set; }
        public string? GroupPicture {  get; set; }
        public GroupChat()
        {
            Members = new List<ChatMember>();
            Messages = new List<Message>();
        }
        public GroupChat(AddGroupChatDTO group,string filePath)
        {
            this.GroupName = group.GroupName;
            Members = new List<ChatMember>();
            Messages = new List<Message>();
            GroupPicture = filePath;
        }
        public void UpdateName(string name)
        {
            this.GroupName = name;
        }
        public void UpdateImage(string path)
        {
            this.GroupPicture = path;
        }
    }
}
