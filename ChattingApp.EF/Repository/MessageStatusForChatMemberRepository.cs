using ChattingApp.CORE.DTO.MessageStatusForChatMemberRepositoryDTO;
using ChattingApp.CORE.DTO.ResultDTO;
using ChattingApp.CORE.Entities;
using ChattingApp.CORE.Interface;
using EF.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace ChattingApp.EF.Repository
{
    public class MessageStatusForChatMemberRepository : IMessageStatusForChatMemberRepository
    {
        private readonly AppDbContext _context;
        public MessageStatusForChatMemberRepository(AppDbContext context)
        {
            _context = context;
        }
        public IntResult Add(int memberId, int messageId)
        {
            var status = new MessageStatusForChatMember(memberId,messageId);
            _context.MessageStatusForChatMember.Add(status);
            var result = SaveChanges();
            if(result.Id == 1)
            {
                result.Id = status.Id;
            }
            return result;
        }
        public ShowStatusofMessageToUserDTO Get(int id)
        {
            var status = _context.MessageStatusForChatMember.Where(x => x.Id == id).Select(x => new ShowStatusofMessageToUserDTO
            {
                Id = x.Id,
                MemberUsername = x.ChatMember.User.UserName,
                MessageId = x.MessageId,
                Recieve = x.IsRecieve,
                Seen = x.IsSeen
            }).FirstOrDefault();
            return status;
        }
        public ShowStatusOfMessageDTO GetStatusOfMessage(int MessageId)
        {
            var statuses = _context.MessageStatusForChatMember.Where(x => x.MessageId == MessageId).Select(x =>new StatusOfUserInMessageDTO{
                MemberUsername = x.ChatMember.User.UserName,
                Recieve = x.IsRecieve,
                Seen = x.IsSeen
            }).ToList();
            var result = new ShowStatusOfMessageDTO { MessageId = MessageId, StatusOfUserInMessage = statuses };
            return result;
        }
        public IntResult MakeItReseaved(int id)
        {
            var status = _context.MessageStatusForChatMember.Find(id);
            if(status is null)
            {
                return new IntResult { Message = "No status has this Id" };
            }
            status.IsRecieve = true;
            return SaveChanges();
        }
        public IntResult MakeItSeen(int id)
        {
            var status = _context.MessageStatusForChatMember.Find(id);
            if (status is null)
            {
                return new IntResult { Message = "No status has this Id" };
            }
            status.IsSeen = true;
            return SaveChanges();
        }
        IntResult SaveChanges()
        {
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
    }
}
