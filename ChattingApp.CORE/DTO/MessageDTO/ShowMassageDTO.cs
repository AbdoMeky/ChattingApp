using ChattingApp.CORE.Enums;
using CORE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChattingApp.CORE.DTO.MessageDTO
{
    public class ShowMassageDTO
    {
        public int Id { get; set; }
        public int? ChatId { get; set; }
        public string? SenderUserName { get; set; }
        public string Content { get; set; }
        public string MediaUrl {  get; set; }
        public bool IsDeleted { get; set; }
        public MessageStatus Status { get; set; }
        public DateTime TimeSended { get; set; }
        public ShowMassageDTO()
        {
            
        }
        public ShowMassageDTO(Message message)
        {
            this.Id = message.Id;
            this.ChatId = message.ChatId;
            this.SenderUserName = (message.Member != null && message.Member.User != null) ? message.Member.User.UserName : "";
            this.Content = message.Content;
            this.MediaUrl = message.MessageMedia?.Url;
            this.IsDeleted = message.IsDeleted;
            this.Status = message.Status;
            this.TimeSended = message.TimeSended;
        }
    }
}
