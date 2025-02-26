using ChattingApp.CORE.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChattingApp.CORE.DTO.MessageDTO
{
    public class AddMessageDTO
    {
        [Required]
        public int ChatId { get; set; }
        [Required]
        public int MemberId { get; set; }
        public string? Content { get; set; }
        public IFormFile? Media {  get; set; }
    }
}
