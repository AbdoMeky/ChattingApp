using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChattingApp.CORE.DTO.ContactDTO
{
    public class AddContactDTO
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string ContactUserId { get; set; }
    }
}
