using ChattingApp.CORE.DTO.MessageDTO;
using ChattingApp.CORE.Entities;
using ChattingApp.CORE.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public int? ChatId { get; set; }
        public int? MemberId { get; set; }
        public string? Content { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsDeletedForSender {  get; set; }
        public MessageStatus Status { get; set; }
        public DateTime TimeSended { get; set; }
        public Chat? Chat { get; set; }
        public ChatMember? Member { get; set; }
        public MessageMedia? MessageMedia { get; set; }
        public List<MessageStatusForChatMember> MessageStatusForChatMembers { get; set; }

        public Message()
        {
            
        }
        public Message(AddMessageDTO message)
        {
            this.ChatId = message.ChatId;
            this.MemberId = message.MemberId;
            this.Content = message.Content;
            this.IsDeleted = false;
            this.Status = MessageStatus.send;
            this.TimeSended= DateTime.UtcNow;
        }
    }

}
