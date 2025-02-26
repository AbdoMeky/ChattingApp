using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChattingApp.CORE.DTO.ContactDTO
{
    public class ShowContactInListDTO
    {
        public int Id { get; set; }
        public string ContactUserName { get; set; }
        public string ContactUserEmail { get; set; }
    }
}
