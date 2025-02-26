using CORE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChattingApp.CORE.DTO.User
{
    public class ShowUserDTO
    {
        public string Name { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string UserName { get; set; }
        public string Email {  get; set; }
        public string PhoneNumber {  get; set; }
        public ShowUserDTO()
        {
            
        }
        public ShowUserDTO(ApplicationUser user)
        {
            this.Name = user.Name;
            this.ProfilePictureUrl = user.ProfilePictureUrl;
            this.UserName = user.UserName;
            this.Email = user.Email;
            this.PhoneNumber = user.PhoneNumber;
        }
    }
}
