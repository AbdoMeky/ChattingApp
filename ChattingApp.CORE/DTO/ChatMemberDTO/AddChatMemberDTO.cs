using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChattingApp.CORE.DTO.ChatMemberDTO
{
    public class AddChatMemberDTO
    {
        public int ChatId {  get; set; }
        public string UserId { get; set; }
        public bool IsAdmin { get; set; }
    }
}
