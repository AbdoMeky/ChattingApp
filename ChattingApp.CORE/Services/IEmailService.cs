using ChattingApp.CORE.DTO.ResultDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChattingApp.CORE.Services
{
    public interface IEmailService
    {
        Task<IntResult> SendEmailAsync(string email, string subject, string body);
        public string GenerateVerificatonCode(int length = 6);
    }
}
