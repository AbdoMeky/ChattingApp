using ChattingApp.CORE.DTO.ChatMemberDTO;
using ChattingApp.CORE.DTO.MessageDTO;
using ChattingApp.CORE.DTO.ResultDTO;
using ChattingApp.CORE.DTO.TwosomeChatDTO;
using ChattingApp.CORE.DTO.User;
using ChattingApp.CORE.Interface;
using CORE.Entities;
using EF.Data;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChattingApp.EF.Repository
{
    public class TwosomeChatRepository : ITwosomeChatRepository
    {
        private readonly AppDbContext _context;
        private readonly IChatMemberRepository _memberRepository;
        private readonly IMessageRepository _messageRepository;
        public TwosomeChatRepository(AppDbContext context, IChatMemberRepository memberRepository, IMessageRepository messageRepository)
        {
            _context = context;
            _memberRepository = memberRepository;
            _messageRepository = messageRepository;

        }
        public IntResult Add(AddTwosomeChatDTO chatUsers)
        {
            var checkChat = Get(chatUsers.UserOneId, chatUsers.UserTwoId);
            if(checkChat is not null)
            {
                return new IntResult { Message = "there is chat between " + checkChat.User1.Email + " and " + checkChat.User2.Email };
            }
            var chat = new TwosomeChat();
            _context.TwosomeChats.Add(chat);
            using(var transaction=_context.Database.BeginTransaction())
            {
                try
                {
                    _context.SaveChanges();
                    var member1=new ChatMember(new AddChatMemberDTO { ChatId = chat.Id, UserId = chatUsers.UserOneId, IsAdmin = true });
                    _context.ChatMembers.Add(member1);
                    var member2 = new ChatMember(new AddChatMemberDTO { ChatId = chat.Id, UserId = chatUsers.UserTwoId, IsAdmin = true });
                    _context.ChatMembers.Add(member2);
                    chat.Members.Add(member1);
                    chat.Members.Add(member2);
                    _context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex) { 
                    return new IntResult { Message = ex.Message };
                }
            }
            return new IntResult { Id=chat.Id };
        }
        public IntResult Delete(int id)
        {
            var chat = _context.TwosomeChats.Find(id);
            if (chat is null)
            {
                return new IntResult { Message = "No Chat has this Id" };
            }
            _context.TwosomeChats.Remove(chat);
            try
            {
                _context.SaveChanges();
            }
            catch(Exception ex)
            {
                return new IntResult { Message = ex.Message };
            }
            return new IntResult { Id = 1 };
        }
        public GetTwosomeChatDTO Get(string userOneId, string userTwoId)
        {
            var result = _context.TwosomeChats.Where(x => 
            (x.Members.FirstOrDefault().UserId==userOneId&&x.Members.Skip(1).FirstOrDefault().UserId==userTwoId)|| (x.Members.FirstOrDefault().UserId == userTwoId && x.Members.Skip(1).FirstOrDefault().UserId == userOneId))
            .Select(x => new GetTwosomeChatDTO
            {
                Id = x.Id,
                User1 = new ShowUserDTO(x.Members.FirstOrDefault().User),
                User2 = new ShowUserDTO(x.Members.Skip(1).FirstOrDefault().User)
            }).FirstOrDefault();
            return result;
        }

        public GetTwosomeChatDTO GetById(int id)
        {
            var result = _context.TwosomeChats.Select(x => new GetTwosomeChatDTO
            {
                Id = x.Id,
                User1 = new ShowUserDTO( x.Members.FirstOrDefault().User),
                User2= new ShowUserDTO(x.Members.Skip(1).FirstOrDefault().User)
            }).FirstOrDefault(x => x.Id == id);
            return result;
        }
        public GetTwosomeChatWithMessagesDTO GetWithMessages(int id)
        {
            var result = _context.TwosomeChats.Select(x => new GetTwosomeChatWithMessagesDTO
            {
                Id = x.Id,
                User1 = new ShowUserDTO(x.Members.FirstOrDefault().User),
                User2 = new ShowUserDTO(x.Members.Skip(1).FirstOrDefault().User)
            }).FirstOrDefault(x => x.Id == id);
            return result;
        }
        /*ShowUserDTO GetUser(ApplicationUser user)
        {

        }*/
    }
}
