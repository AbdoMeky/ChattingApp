using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChattingApp.CORE.DTO.MessageStatusForChatMemberRepositoryDTO
{
    public class StatusOfUserInMessageDTO
    {
        public string MemberUsername { get; set; }
        public bool Recieve { get; set; }
        public bool Seen { get; set; }
    }
}
