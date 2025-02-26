using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChattingApp.CORE.DTO.User
{
    public class UpdateProfilePictureUrlDTO
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public IFormFile Image { get; set; }
    }
}
