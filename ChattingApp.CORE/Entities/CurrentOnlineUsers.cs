using CORE.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChattingApp.CORE.Entities
{
    public class CurrentOnlineUsers
    {
        public int Id {  get; set; }
        public string UserID {  get; set; }
        public string ConnectionId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
