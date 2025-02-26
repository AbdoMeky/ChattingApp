using ChattingApp.CORE.Helper;
using ChattingApp.CORE.Services;
using CORE.Entities;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ChattingApp.CORE.DTO.ResultDTO;

namespace ChattingApp.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            this.emailSettings = emailSettings.Value;
        }
        public string GenerateVerificatonCode(int length = 6)
        {
            var randomNumber = new byte[length];
            RandomNumberGenerator.Fill(randomNumber);
            return string.Join("", randomNumber.Select(b => (b % 10).ToString()));
        }

        public async Task<IntResult> SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                string? email = emailSettings.Email;
                var password = emailSettings.Password;

                using (var client = new SmtpClient(emailSettings.SmtpServer, 587))
                {
                    client.Credentials = new NetworkCredential(email, password);
                    client.UseDefaultCredentials = false;
                    client.EnableSsl = true;
                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(email),
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true,
                    };
                    mailMessage.To.Add(toEmail);
                    await client.SendMailAsync(mailMessage);
                    return new IntResult { Id = 1 };
                }
            }
            catch(Exception ex)
            {
                return new IntResult { Message = ex.Message };
            }
        }
    }
}
