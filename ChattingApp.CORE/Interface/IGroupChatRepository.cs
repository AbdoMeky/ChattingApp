using ChattingApp.CORE.DTO.GroupChatDTO;
using ChattingApp.CORE.DTO.ResultDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChattingApp.CORE.Interface
{
    public interface IGroupChatRepository
    {
        Task<IntResult> AddAsync(AddGroupChatDTO group);
        IntResult Delete(int id);
        Task<IntResult> Update(UpdateGroupChatDTO group);
        IntResult AddAdmin(int  adminId);
        IntResult DeleteAdmin(int adminId);
        IntResult AddMember(string userId, int chatId);
        IntResult DeleteMember(int memberId);
        ShowGroupChatDTO Get(int id);
        ShowGroupChatWithMessagesDTO GetWithMessages(int id);
    }
}
