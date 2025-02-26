using ChattingApp.CORE.DTO.AuthDTO;
using ChattingApp.CORE.DTO.GeneralChatDTO;
using ChattingApp.CORE.DTO.ResultDTO;
using ChattingApp.CORE.DTO.User;
using CORE.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChattingApp.CORE.Interface
{
    public interface IUserRepository
    {
        Task<StringResult> Add(RegisterDTO User);
        //Task<StringResult> Update(UpdateUserDTO User);
        Task<StringResult> ChangeProfilePictureUrl(UpdateProfilePictureUrlDTO oldUser);
        StringResult ChangeName(UpdateNameDTO oldUser);
        IntResult Delete(string id);
        ApplicationUser Get(string id);
        ApplicationUser GetWithRefreshToken(string id);
        Task<StringResult> AddImage(IFormFile image, string id);
        void DeleteImage(string image);
        ShowUserDTO GetUser(string id);
        List<ShowUserDTO> Search(string searchKey);
        List<SHowExternalChatForUSerDTO> GetExternalChatsForUser(string id);
        List<int> ChatsIdForUser(string id);
        string UserName(string id);
    }
}
