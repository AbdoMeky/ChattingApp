using ChattingApp.CORE.DTO.ChatMemberDTO;
using ChattingApp.CORE.DTO.ResultDTO;
using ChattingApp.CORE.Interface;
using CORE.Entities;
using EF.Data;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ChattingApp.EF.Repository
{
    public class ChatMemberRepository : IChatMemberRepository
    {
        private readonly AppDbContext _context;
        public ChatMemberRepository(AppDbContext context)
        {
            _context = context;
        }
        public ChatMember Add(AddChatMemberDTO member)
        {
            var newMember = new ChatMember(member);
            return newMember;
        }
        public IntResult AddAdminToMember(string UserId, int ChatId)
        {
            var member =_context.ChatMembers.FirstOrDefault(x=>x.UserId==UserId && x.ChatId==ChatId);
            member.AddAdmin();
            return saveChanges();
        }

        public IntResult AddAdminToMember(int id)
        {
            var member = _context.ChatMembers.Find(id);
            if (member is null)
            {
                return new IntResult { Message = "No ember has this Id" };
            }
            if (member.IsAdmin == true)
            {
                return new IntResult { Message = "The user is already admin" };
            }
            member.AddAdmin();
            return saveChanges();
        }

        public IntResult Delete(int MemberId)
        {
            var member = _context.ChatMembers.Find(MemberId);
            if(member is null)
            {
                return new IntResult { Message = "No member has this Id" };
            }
            _context.ChatMembers.Remove(member);
            var result= saveChanges();
            if (result.Id == 0)
            {
                return result;
            }
            result.Id = member.ChatId is not null? member.ChatId.Value:0;
            return result;
        }
        public IntResult Delete(int chatId,string userId)
        {
            var member = _context.ChatMembers.FirstOrDefault(x => x.ChatId == chatId && x.UserId == userId);
            if (member is null)
            {
                return new IntResult { Message = "this user not in the chat" };
            }
            _context.ChatMembers.Remove(member);
            var result = saveChanges();
            if (result.Id == 0)
            {
                return result;
            }
            result.Id = member.ChatId is not null ? member.ChatId.Value : 0;
            return result;
        }
        public List<ShowChatMemberDTO> GetMembersInGroupChat(int chatId)
        {
            var result = _context.ChatMembers.Where(x => x.ChatId == chatId).Select(x => new ShowChatMemberDTO
            {
                Id = x.Id,
                IsAdmin = x.IsAdmin,
                Name = x.User.Name,
                ProfilePictureUrl = x.User.ProfilePictureUrl
            }).OrderByDescending(x=>x.IsAdmin).ToList();
            return result;
        }

        public IntResult RemoveAdminFromMember(int id)
        {
            var member = _context.ChatMembers.Find(id);
            if(member is null)
            {
                return new IntResult { Message = "No ember has this Id" };
            }
            if (member.IsAdmin == false)
            {
                return new IntResult { Message = "you already are not admin" };
            }
            member.RemoveAdmin();
            return saveChanges();
        }

        public IntResult RemoveAdminFromMember(string UserId, int ChatId)
        {
            var member = _context.ChatMembers.FirstOrDefault(x => x.UserId == UserId && x.ChatId == ChatId);
            if (member is null)
            {
                return new IntResult { Message = "No ember has this Id" };
            }
            if (member.IsAdmin == false)
            {
                return new IntResult { Message = "you already are not admin" };
            }
            member.RemoveAdmin();
            return saveChanges();
        }
        IntResult saveChanges()
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
