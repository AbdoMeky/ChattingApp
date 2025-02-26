using ChattingApp.CORE.DTO.AuthDTO;
using ChattingApp.CORE.DTO.ResultDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChattingApp.CORE.Services
{
    public interface IAuthService
    {
        Task<AuthResultDTO> LogIn(LogInDTO logInDTO);//
        Task<AuthResultDTO> RegisteUser(RegisterDTO sellerDTO);//
        Task<IntResult> ChangePasswordAsync(ChangePasswordDTO newPassword);//
        Task<AuthResultDTO> CheckRefreshTokenAndRevoke(string token);//
        //Task<IntResult> SendConfirmationEmail(string email);
        Task<IntResult> ConfirmEmailAsync(VerifyCodeDTO request);//
        Task<IntResult> ForgetPasswordAsync(string email);//
        Task<AuthResultDTO> VerifyResetCodeAsync(VerifyCodeDTO request);//
        Task<IntResult> ResetPasswordAsync(ResetPasswordDTO resetPasswordDTO);//
        Task<IntResult> ResendConfirmationCodeAsync(string email);//
        Task<IntResult> ResendResetPasswordCodeAsync(string Email);//
        Task<bool> RevokeRefreshToken(string token);//
    }
}
