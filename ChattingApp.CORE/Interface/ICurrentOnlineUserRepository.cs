using ChattingApp.CORE.DTO.ResultDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChattingApp.CORE.Interface
{
    public interface ICurrentOnlineUserRepository
    {
        IntResult Add(string connectionId, string userId);
        IntResult Delete(string connectionId, string userId);
    }
}
