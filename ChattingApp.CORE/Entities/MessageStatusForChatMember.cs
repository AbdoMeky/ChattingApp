using CORE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChattingApp.CORE.Entities
{
    public class MessageStatusForChatMember
    {
        public int Id {  get; set; }
        public int MessageId {  get; set; }
        public int MemberId {  get; set; }
        public bool IsRecieve {  get; set; }
        public bool IsSeen {  get; set; }
        public bool IsDeletedForMember {  get; set; }
        public Message Message { get; set; }
        public ChatMember ChatMember { get; set; }
        public MessageStatusForChatMember()
        {
            
        }
        public MessageStatusForChatMember(int memberId,int messageId)
        {
            this.MemberId = memberId;
            this.MessageId = messageId;
            this.IsRecieve = false;
            this.IsSeen = false;
            this.IsDeletedForMember = false;
        }
    }
}
