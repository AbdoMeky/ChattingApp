using ChattingApp.CORE.DTO.ResultDTO;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChattingApp.CORE.Interface
{
    public interface IMediaMessageRepository
    {
        Task<IntResult> Add(IFormFile media, int MessageId);
    }
}
