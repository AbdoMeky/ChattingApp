using ChattingApp.CORE.DTO.ResultDTO;
using ChattingApp.CORE.DTO.TwosomeChatDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChattingApp.CORE.Interface
{
    public interface ITwosomeChatRepository
    {
        IntResult Add(AddTwosomeChatDTO chatUsers);
        GetTwosomeChatDTO GetById(int id);
        GetTwosomeChatDTO Get(string userOneId, string userTwoId);
        GetTwosomeChatWithMessagesDTO GetWithMessages(int id);
        IntResult Delete(int id);
    }
}
