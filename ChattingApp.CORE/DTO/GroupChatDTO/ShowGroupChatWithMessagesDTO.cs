using ChattingApp.CORE.DTO.ChatMemberDTO;
using ChattingApp.CORE.DTO.MessageDTO;
using ChattingApp.CORE.DTO.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChattingApp.CORE.DTO.GroupChatDTO
{
    public class ShowGroupChatWithMessagesDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ShowChatMemberDTO> Members { get; set; }
        public List<ShowMassageInChatDTO> Messages { get; set; }
        public ShowGroupChatWithMessagesDTO()
        {
            Members = new List<ShowChatMemberDTO>();
            Messages= new List<ShowMassageInChatDTO>();
        }
    }
}
