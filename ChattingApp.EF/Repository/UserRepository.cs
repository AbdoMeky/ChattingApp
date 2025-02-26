using ChattingApp.CORE.DTO.AuthDTO;
using ChattingApp.CORE.DTO.GeneralChatDTO;
using ChattingApp.CORE.DTO.ResultDTO;
using ChattingApp.CORE.DTO.User;
using ChattingApp.CORE.Interface;
using CORE.Entities;
using EF.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ChattingApp.EF.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly string _storagePath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedImagesForUserImage");

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<StringResult> Add(RegisterDTO user)
        {
            if (!Directory.Exists(_storagePath))
            {
                Directory.CreateDirectory(_storagePath);
            }
            var filePath = chickImagePath(user.Image);
            if (!string.IsNullOrEmpty(filePath.Message))
            {
                return new StringResult() { Message = filePath.Message };
            }
            var newUser = new ApplicationUser(user, filePath.Id);
            _context.Users.Add(newUser);
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync();
                    if (!string.IsNullOrEmpty(filePath.Id))
                    {
                        using (var stream = new FileStream(filePath.Id, FileMode.Create))
                        {
                            await user.Image.CopyToAsync(stream);
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    return new StringResult() { Message = ex.Message };
                }
            }
            return new StringResult() { Id = newUser.Id };
        }

        public IntResult Delete(string id)
        {
            var user = Get(id);
            if (user is null)
            {
                return new IntResult { Id = 0, Message = "No User has this Id" };
            }
            string oldPath = user.ProfilePictureUrl;
            _context.Users.Remove(user);
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.SaveChanges();
                    if (File.Exists(oldPath))
                    {
                        File.Delete(oldPath);
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    return new IntResult() { Message = ex.Message };
                }
            }
            return new IntResult { Id = 1 };
        }

        public ApplicationUser Get(string id)
        {
            return _context.Users.Find(id);
        }

        public async Task<StringResult> Update(UpdateUserDTO user)
        {
            var filePath = chickImagePath(user.Image);
            if (!string.IsNullOrEmpty(filePath.Message))
            {
                return new StringResult() { Message = filePath.Message };
            }
            var oldUser = Get(user.Id);
            if(user is null)
            {
                return new StringResult { Message = "No user has this Id" };
            }
            var oldImagePath = oldUser.ProfilePictureUrl;
            oldUser.Update(user.Name, filePath.Id);
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync();
                    if (File.Exists(oldImagePath))
                    {
                        File.Delete(oldImagePath);
                    }
                    if (!string.IsNullOrEmpty(filePath.Id))
                    {
                        using (var stream = new FileStream(filePath.Id, FileMode.Create))
                        {
                            await user.Image.CopyToAsync(stream);
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    return new StringResult() { Message = ex.Message };
                }
            }
            return new StringResult { Id = user.Id };
        }
        public StringResult ChangeName(UpdateNameDTO oldUser)
        {
            var user = Get(oldUser.Id);
            if(user is null)
            {
                return new StringResult { Message = "No user has this Id" };
            }
            user.ChangeName(oldUser.Name);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return new StringResult { Message = ex.Message };
            }
            return new StringResult { Id = user.Id };
        }
        public async Task<StringResult> ChangeProfilePictureUrl(UpdateProfilePictureUrlDTO oldUser)
        {
            var filePath = chickImagePath(oldUser.Image);
            if (!string.IsNullOrEmpty(filePath.Message))
            {
                return new StringResult() { Message = filePath.Message };
            }
            var user = Get(oldUser.Id);
            if (user is null)
            {
                return new StringResult { Message = "No user has this Id" };
            }
            var oldImagePath = user.ProfilePictureUrl;
            user.ChangeProfilePicture(filePath.Id);
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync();
                    if (File.Exists(oldImagePath))
                    {
                        File.Delete(oldImagePath);
                    }
                    if (!string.IsNullOrEmpty(filePath.Id))
                    {
                        using (var stream = new FileStream(filePath.Id, FileMode.Create))
                        {
                            await oldUser.Image.CopyToAsync(stream);
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    return new StringResult() { Message = ex.Message };
                }
            }
            return new StringResult { Id = user.Id };
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
        public async Task<StringResult> AddImage(IFormFile image, string id)
        {
            if (!Directory.Exists(_storagePath))
            {
                Directory.CreateDirectory(_storagePath);
            }
            var filePath = chickImagePath(image);
            if (!string.IsNullOrEmpty(filePath.Message))
            {
                return new StringResult() { Message = filePath.Message };
            }
            var user = _context.Users.Find(id);
            user.ProfilePictureUrl = filePath.Id;
            try
            {

                await _context.SaveChangesAsync();
                if (!string.IsNullOrEmpty(filePath.Id))
                {
                    using (var stream = new FileStream(filePath.Id, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }
                }
            }
            catch (Exception ex)
            {
                return new StringResult() { Message = ex.Message };
            }
            return new StringResult() { Id = filePath.Id };
        }
        public ApplicationUser GetWithRefreshToken(string id)
        {
            return _context.Users.Include(x => x.RefreshTokens).FirstOrDefault(x => x.Id == id);
        }
        public void DeleteImage(string oldImagePath)
        {
            if (File.Exists(oldImagePath))
            {
                File.Delete(oldImagePath);
            }
        }
        public ShowUserDTO GetUser(string id)
        {
            var user = _context.Users.Find(id);
            if(user is null)
            {
                return null;
            }
            return new ShowUserDTO(user);
        }
        public List<ShowUserDTO> Search(string searchKey)
        {
            string usernamePattern = @"^[A-Za-z][A-Za-z0-9]*$";
            string numberPattern = @"^[0-9]+$";
            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            if (Regex.IsMatch(searchKey, usernamePattern))
            {
                var result = _context.Users.Where(x => x.UserName.Contains(searchKey)).Take(10).Select(x => new ShowUserDTO(x));
                return result.ToList();
            }
            else if(Regex.IsMatch(searchKey, numberPattern))
            {
                var result = _context.Users.Where(x => x.PhoneNumber.Contains(searchKey)).Take(10).Select(x => new ShowUserDTO(x));
                return result.ToList();
            }
            else if(Regex.IsMatch(searchKey, emailPattern))
            {
                var result = _context.Users.Where(x => x.Email.Contains(searchKey)).Take(10).Select(x => new ShowUserDTO(x));
                return result.ToList();
            }
            return new List<ShowUserDTO>();
        }

        public List<SHowExternalChatForUSerDTO> GetExternalChatsForUser(string id)
        {
            var chats = _context.Chats
                .Where(x => x.Members.Any(m => m.UserId == id))
                .Select(x => x is GroupChat ?
                     new SHowExternalChatForUSerDTO
                    {
                        Id = x.Id,
                        Name = ((GroupChat)x).GroupName,
                        ChatPicture = ((GroupChat)x).GroupPicture
                    }
                    : new SHowExternalChatForUSerDTO
                    {
                        Id = x.Id,
                        Name = x.Members.Where(m => m.UserId != id).Select(x=>x.User.Name).FirstOrDefault(),
                        ChatPicture = x.Members.Where(m => m.UserId != id).Select(x => x.User.ProfilePictureUrl).FirstOrDefault()
                    })
                .ToList();

            return chats;
        }
        public List<int> ChatsIdForUser(string id)
        {
            return _context.Chats.Where(x=> x.Members.Any(m=>m.UserId == id)).Select(x=>x.Id).ToList();
        }
        public string UserName(string id)
        {
            return _context.Users.Find(id).Name;
        }
    }
}

