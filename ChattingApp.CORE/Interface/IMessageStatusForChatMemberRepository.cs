using ChattingApp.CORE.DTO.MessageStatusForChatMemberRepositoryDTO;
using ChattingApp.CORE.DTO.ResultDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChattingApp.CORE.Interface
{
    public interface IMessageStatusForChatMemberRepository
    {
        IntResult Add(int memberId, int messageId);
        IntResult MakeItReseaved(int id);
        IntResult MakeItSeen(int id);
        ShowStatusofMessageToUserDTO Get(int id);
        ShowStatusOfMessageDTO GetStatusOfMessage(int MessageId);
    }
}
