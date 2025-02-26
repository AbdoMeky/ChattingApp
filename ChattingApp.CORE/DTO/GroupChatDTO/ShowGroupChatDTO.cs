using ChattingApp.CORE.DTO.ChatMemberDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChattingApp.CORE.DTO.GroupChatDTO
{
    public class ShowGroupChatDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ShowChatMemberDTO> Members { get; set; }
        public ShowGroupChatDTO()
        {
            Members = new List<ShowChatMemberDTO>();
        }
    }
}
