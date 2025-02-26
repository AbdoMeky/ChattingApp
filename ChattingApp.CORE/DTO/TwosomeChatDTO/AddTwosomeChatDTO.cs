using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChattingApp.CORE.DTO.TwosomeChatDTO
{
    public class AddTwosomeChatDTO
    {
        [Required]
        public string UserOneId { get; set; }
        [Required]
        public string UserTwoId { get; set; }
    }
}
