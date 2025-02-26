using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChattingApp.CORE.DTO.ContactDTO
{
    public class ShowContactOfUserDTO
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<ShowContactInListDTO> Contacts {  get; set; }
        public ShowContactOfUserDTO()
        {
            Contacts=new List<ShowContactInListDTO>();
        }
        public ShowContactOfUserDTO(string name,string email, List<ShowContactInListDTO>contacts)
        {
            UserName = name;
            Email = email;
            Contacts = contacts;
        }
    }
}
