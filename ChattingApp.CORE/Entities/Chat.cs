using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Entities
{
    public class Chat
    {
        public int Id { get; set; }
        public ICollection<Message>? Messages { get; set; }
        public ICollection<ChatMember> Members { get; set; }
    }

}
