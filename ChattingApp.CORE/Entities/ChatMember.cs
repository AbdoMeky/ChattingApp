using ChattingApp.CORE.DTO.ChatMemberDTO;
using ChattingApp.CORE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Entities
{
    public class ChatMember
    {
        public int Id { get; set; }
        public int? ChatId { get; set; }
        public string UserId { get; set; }
        public bool IsAdmin {  get; set; }
        public DateTime AddedTime { get; set; }
        public List<MessageStatusForChatMember>? messageStatusForChatMembers { get; set; }
        public List<Message>?Messages { get; set; }
        public Chat? Chat { get; set; }
        public ApplicationUser User { get; set; }
        public ChatMember()
        {
            this.Messages = new List<Message>();
            this.messageStatusForChatMembers = new List<MessageStatusForChatMember>();
        }
        public ChatMember(AddChatMemberDTO member)
        {
            this.ChatId = member.ChatId;
            this.UserId = member.UserId;
            this.IsAdmin = member.IsAdmin;
            this.AddedTime = DateTime.UtcNow;
            this.Messages = new List<Message>();
            this.messageStatusForChatMembers = new List<MessageStatusForChatMember>();
        }
        public void AddAdmin()
        {
            this.IsAdmin = true;
        }
        public void RemoveAdmin()
        {
            this.IsAdmin = false;
        }
    }
}
