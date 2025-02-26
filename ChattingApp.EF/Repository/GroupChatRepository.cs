using ChattingApp.CORE.DTO.ChatMemberDTO;
using ChattingApp.CORE.DTO.GroupChatDTO;
using ChattingApp.CORE.DTO.ResultDTO;
using ChattingApp.CORE.Interface;
using CORE.Entities;
using EF.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChattingApp.EF.Repository
{
    public class GroupChatRepository : IGroupChatRepository
    {
        private readonly AppDbContext _context;
        private readonly IChatMemberRepository _chatMemberRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly string _storagePath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedImagesForGroupImage");
        public GroupChatRepository(AppDbContext context, IChatMemberRepository chatMemberRepository, IMessageRepository messageRepository)
        {
            _context = context;
            _chatMemberRepository = chatMemberRepository;
            _messageRepository = messageRepository;
        }
        public async Task<IntResult> AddAsync(AddGroupChatDTO group)
        {
            if (!Directory.Exists(_storagePath))
            {
                Directory.CreateDirectory(_storagePath);
            }
            var filePath = chickImagePath(group.Image);
            if (!string.IsNullOrEmpty(filePath.Message))
            {
                return new IntResult() { Message = filePath.Message };
            }
            var newGroup = new GroupChat(group, filePath.Id);
            _context.GroupChats.Add(newGroup);
            using (var transaction = _context.Database.BeginTransaction()) {
                try
                {
                    _context.SaveChanges();
                    var admin = _chatMemberRepository.Add(new AddChatMemberDTO { ChatId = newGroup.Id, UserId = group.AdminId, IsAdmin = true });
                    newGroup.Members.Add(admin);
                    if (!string.IsNullOrEmpty(filePath.Id))
                    {
                        using (var stream = new FileStream(filePath.Id, FileMode.Create))
                        {
                            await group.Image.CopyToAsync(stream);
                        }
                    }
                    _context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    return new IntResult { Message= ex.Message };
                }
            }
            return new IntResult { Id = newGroup.Id };
        }
        public async Task<IntResult> Update(UpdateGroupChatDTO group)
        {
            var newGroup = _context.GroupChats.Find(group.Id);
            if (newGroup is null)
            {
                return new IntResult { Message = "No group has this id" };
            }
            if (!Directory.Exists(_storagePath))
            {
                Directory.CreateDirectory(_storagePath);
            }
            var filePath = chickImagePath(group.Image);
            if (!string.IsNullOrEmpty(filePath.Message))
            {
                return new IntResult() { Message = filePath.Message };
            }
            newGroup.UpdateName(group.Name);
            string oldPath="";
            if (!string.IsNullOrEmpty(filePath.Id))
            {
                oldPath = newGroup.GroupPicture;
                newGroup.UpdateImage(filePath.Id);
            }
            try
            {
                _context.SaveChanges();
                if (!string.IsNullOrEmpty(filePath.Id))
                {
                    using (var stream = new FileStream(filePath.Id, FileMode.Create))
                    {
                        await group.Image.CopyToAsync(stream);
                    }
                    if (File.Exists(oldPath))
                    {
                        File.Delete(oldPath);
                    }
                }
            }
            catch (Exception ex)
            {
                return new IntResult { Message = ex.Message };
            }
            return new IntResult { Id = 1 };
        }
        public IntResult AddAdmin(int adminId)
        {
            return _chatMemberRepository.AddAdminToMember(adminId);
        }
        public IntResult AddMember(string userId,int chatId)
        {
            if(_context.ChatMembers.FirstOrDefault(x=>x.UserId==userId && chatId == x.ChatId) is not null)
            {
                return new IntResult { Message = _context.Users.Find(userId).Name + " is already in " + _context.GroupChats.Find(chatId).GroupName + " group" };
            }
            var member=_chatMemberRepository.Add(new AddChatMemberDTO { ChatId = chatId, UserId = userId });
            _context.GroupChats.Find(chatId).Members.Add(member);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return new IntResult { Message = ex.Message };
            }
            return new IntResult { Id = member.Id };
        }
        public IntResult Delete(int id)
        {
            var group = _context.GroupChats.Include(x => x.Members).FirstOrDefault(x => x.Id == id);
            if (group is null)
            {
                return new IntResult { Message = "No Group has this Id" };
            }
            _context.GroupChats.Remove(group);
            try
            {
                foreach (var member in group.Members)
                {
                    _context.ChatMembers.Remove(member);
                }
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return new IntResult { Message = ex.Message };
            }
            return new IntResult { Id = 1 };
        }
        public IntResult DeleteAdmin(int adminId)
        {
            return _chatMemberRepository.RemoveAdminFromMember(adminId);
        }
        public IntResult DeleteMember(int memberId)
        {
            using (var transaction = _context.Database.BeginTransaction()) {
                try
                {
                    var result = _chatMemberRepository.Delete(memberId);
                    if (result.Id == 0)
                    {
                        return result;
                    }
                    var group = _context.GroupChats.Include(x => x.Members).FirstOrDefault(x => x.Id == result.Id);
                    if (group.Members.Count == 0)
                    {
                        _context.GroupChats.Remove(group);
                        _context.SaveChanges();
                    }
                    else
                    {
                        bool check = false;
                        int OldMemberId = 0;
                        DateTime OldMemberEntered = DateTime.UtcNow;
                        foreach (var member in group.Members)
                        {
                            if (member.IsAdmin)
                            {
                                check = true;
                                break;
                            }
                            if (member.AddedTime < OldMemberEntered)
                            {
                                OldMemberEntered = member.AddedTime;
                                OldMemberId = member.Id;
                            }
                        }
                        if (!check)
                        {
                            var AdminResult = _chatMemberRepository.AddAdminToMember(OldMemberId);
                            if (AdminResult.Id == 0)
                            {
                                return AdminResult;
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
            return new IntResult { Id = 1 };
        }

        public ShowGroupChatDTO Get(int id)
        {
            var result=_context.GroupChats.Where(x=>x.Id==id).Select(x => new ShowGroupChatDTO
            {
                Id = x.Id,
                Name = x.GroupName,
                Members = _chatMemberRepository.GetMembersInGroupChat(id)
            }).FirstOrDefault();
            return result;
        }

        public ShowGroupChatWithMessagesDTO GetWithMessages(int id)
        {
            var result = _context.GroupChats.Where(x => x.Id == id).Select(x => new ShowGroupChatWithMessagesDTO
            {
                Id = x.Id,
                Name = x.GroupName,
                Members = _chatMemberRepository.GetMembersInGroupChat(id),
                Messages = _messageRepository.ShowMessageInChat(id)
            }).FirstOrDefault();
            return result;
        }
        StringResult chickImagePath(IFormFile file)
        {
            if (file is null)
            {
                return new StringResult();
            }
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            var contentType = file.ContentType.ToLower();
            if (!allowedExtensions.Contains(fileExtension) || !contentType.StartsWith("image/"))
            {
                return new StringResult { Message = "Invalid file type, Only images are allowed" };
            }
            var fileName = Path.GetRandomFileName() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(_storagePath, fileName);
            return new StringResult { Id = filePath };
        }
    }
}
