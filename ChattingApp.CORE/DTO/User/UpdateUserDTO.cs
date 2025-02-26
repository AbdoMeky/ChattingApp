using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChattingApp.CORE.DTO.User
{
    public class UpdateUserDTO
    {
        [Required]
        public string Id {  get; set; }
        public string? Name { get; set; }
        public IFormFile? Image { get; set; }
    }
}
