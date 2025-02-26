using ChattingApp.CORE.DTO.MessageDTO;
using ChattingApp.CORE.DTO.ResultDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChattingApp.CORE.Interface
{
    public interface IMessageRepository
    {
        Task<IntResult> Add(AddMessageDTO message);
        IntResult Delete(int id);
        IntResult MakeItSeen(int id);
        ShowMassageDTO Get(int id);
        List<ShowMassageInChatDTO> ShowMessageInChat(int chatId);
    }
}
