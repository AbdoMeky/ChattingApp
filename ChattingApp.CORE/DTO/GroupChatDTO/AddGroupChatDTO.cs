using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChattingApp.CORE.DTO.GroupChatDTO
{
    public class AddGroupChatDTO
    {
        [Required]
        public string GroupName { get; set; }
        [Required]
        public string AdminId { get; set; }
        public IFormFile? Image {  get; set; }
    }
}
