using ChattingApp.CORE.DTO.User;
using CORE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChattingApp.CORE.DTO.TwosomeChatDTO
{
    public class GetTwosomeChatDTO
    {
        public int Id { get; set; }
        public ShowUserDTO User1 { get; set; }
        public ShowUserDTO User2 { get; set; }
        public GetTwosomeChatDTO()
        {
            
        }
        public GetTwosomeChatDTO(TwosomeChat chat)
        {
            this.Id = chat.Id;
            User1 = new ShowUserDTO(chat.Members.FirstOrDefault().User);
            User2= new ShowUserDTO(chat.Members.LastOrDefault().User);
        }
    }
}
