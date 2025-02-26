using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Entities
{
    public class MessageMedia
    {
        public int Id { get; set; }
        public int MessageId { get; set; }
        public string Url { get; set; }
        // Navigation Properties
        public Message Message { get; set; }
        public MessageMedia()
        {
            
        }
        public MessageMedia(string url,int MessageId)
        {
            this.Url = url;
            this.MessageId = MessageId;
            Message = new Message();
        }
    }
}
