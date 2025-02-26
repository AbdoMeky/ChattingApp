using ChattingApp.CORE.DTO.MessageDTO;
using ChattingApp.CORE.DTO.ResultDTO;
using ChattingApp.CORE.Enums;
using ChattingApp.CORE.Interface;
using CORE.Entities;
using EF.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChattingApp.EF.Repository
{
    public class MessageRepository : IMessageRepository
    {
        private readonly AppDbContext _context;
        private readonly IMediaMessageRepository _mediaRepository;
        private readonly IMessageStatusForChatMemberRepository _messageStatus;
        private readonly IChatMemberRepository _chatMemberRepository;
        public MessageRepository(AppDbContext context,
            IMediaMessageRepository mediaRepository,
            IMessageStatusForChatMemberRepository messageStatus,
            IChatMemberRepository chatMemberRepository)
        {
            this._context = context;
            this._mediaRepository = mediaRepository;
            this._messageStatus = messageStatus;
            _chatMemberRepository = chatMemberRepository;
        }
        public async Task<IntResult> Add(AddMessageDTO message)
        {
            var newMessage = new Message(message);
            _context.Chats.Find(message.ChatId).Messages.Add(newMessage);
            using(var transaction=_context.Database.BeginTransaction())
            {
                try
                {
                    _context.SaveChanges();
                    if (message.Media is not null)
                    {
                        var result = await _mediaRepository.Add(message.Media, newMessage.Id);
                        if (result.Id == 0)
                        {
                            return result;
                        }
                    }
                    var members = _chatMemberRepository.GetMembersInGroupChat(message.ChatId);
                    foreach (var member in members)
                    {
                        if (member.Id != newMessage.MemberId)
                        {
                            var result = _messageStatus.Add(member.Id, newMessage.Id);
                            if (result.Id == 0)
                            {
                                return new IntResult { Message = result.Message };
                            }
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    return new IntResult { Message = ex.Message };
                }
            }
            return new IntResult { Id = newMessage.Id };
        }
        public IntResult Delete(int id)
        {
            var message = _context.Messages.Find(id);
            if (message is null)
            {
                return new IntResult { Message = "No message has this Id" };
            }
            message.IsDeleted = true;
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return new IntResult { Message = ex.Message };
            }
            return new IntResult { Id = 1 };
        }
        public IntResult MakeItSeen(int id)
        {
            var message = _context.Messages.Find(id);
            if (message is null)
            {
                return new IntResult { Message = "No message has this Id" };
            }
            message.Status = MessageStatus.seen;
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return new IntResult { Message = ex.Message };
            }
            return new IntResult { Id = 1 };
        }
        public ShowMassageDTO Get(int id)
        {
            var message = _context.Messages.Select(x => new ShowMassageDTO
            {
                ChatId = x.ChatId,
                Content = x.Content,
                SenderUserName =(x.Member != null&&x.Member.User !=null)? x.Member.User.UserName:"",
                Status = x.Status,
                Id = x.Id,
                TimeSended = x.TimeSended,
                IsDeleted = x.IsDeleted,
                MediaUrl = x.MessageMedia == null ? "" : x.MessageMedia.Url
            }).FirstOrDefault(x => x.Id == id);
            return message;
        }
        public List<ShowMassageInChatDTO> ShowMessageInChat(int chatId)
        {
            List<ShowMassageInChatDTO> chatMessages = _context.Messages.Where(x => x.ChatId == chatId)
                .OrderBy(x => x.TimeSended)
                .Select(x => new ShowMassageInChatDTO
                {
                    Content = x.Content,
                    Id = x.Id,
                    IsDeleted = x.IsDeleted,
                    SenderUserName = (x.Member != null && x.Member.User != null) ? x.Member.User.UserName : "",
                    Status = x.Status,
                    TimeSended = x.TimeSended,
                    MediaUrl = x.MessageMedia == null ? "" : x.MessageMedia.Url
                }).ToList();
            return chatMessages;
        }
    }
}
