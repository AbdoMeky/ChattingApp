using ChattingApp.CORE.DTO.ChatMemberDTO;
using ChattingApp.CORE.DTO.ResultDTO;
using CORE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChattingApp.CORE.Interface
{
    public interface IChatMemberRepository
    {
        ChatMember Add(AddChatMemberDTO member);
        IntResult RemoveAdminFromMember(int id);
        IntResult RemoveAdminFromMember(string UserId,int ChatId);
        IntResult AddAdminToMember(string UserId, int ChatId);
        IntResult AddAdminToMember(int id);
        IntResult Delete(int MemberId);
        public IntResult Delete(int chatId, string userId);
        List<ShowChatMemberDTO> GetMembersInGroupChat(int chatId);
    }
}
