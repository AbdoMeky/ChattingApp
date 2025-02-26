using ChattingApp.CORE.DTO.AuthDTO;
using ChattingApp.CORE.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using System.Globalization;

namespace ChattingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountingController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AccountingController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("RegisteUser")]
        public async Task<ActionResult> RegisteUser(RegisterDTO user)//
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.RegisteUser(user);
                if (result.IsAuthenticated)
                {
                    if (!string.IsNullOrEmpty(result.RefreshToken))
                    {
                        SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpired);
                    }
                    return Ok(result);
                }
                return BadRequest(result.Message);
            }
            return BadRequest(ModelState);
        }
        [HttpPost("LogIn")]
        public async Task<ActionResult> LogIn(LogInDTO logInDTO)//
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.LogIn(logInDTO);
                if (result.IsAuthenticated)
                {
                    if (!string.IsNullOrEmpty(result.RefreshToken))
                    {
                        SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpired);
                    }
                    return Ok(result);
                }
                return BadRequest(result.Message);
            }
            return BadRequest(ModelState);
        }
        [HttpGet("RevokeAndInvoke")]
        public async Task<ActionResult> RevokeAndInvoke()//
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var result = await _authService.CheckRefreshTokenAndRevoke(refreshToken);
            if (result.IsAuthenticated)
            {
                if (!string.IsNullOrEmpty(result.RefreshToken))
                {
                    SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpired);
                }
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
        [HttpGet("LogOut")]
        public async Task<ActionResult> LogOut()//
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var result = await _authService.RevokeRefreshToken(refreshToken);
            if (result)
            {
                return Ok();
            }
            return BadRequest("no valid refresh token to revoke");
        }
        [HttpPost("ChangePassword")]
        public async Task<ActionResult>ChangePassword(ChangePasswordDTO newPassword)
        {
            if (ModelState.IsValid)
            {
                var result =await _authService.ChangePasswordAsync(newPassword);
                if (result.Id==1)
                {
                    return Ok();
                }
                return BadRequest(result.Message);
            }
            return BadRequest(ModelState);
        }
        [HttpPost("ConfirmEmail")]
        public async Task<ActionResult> ConfirmEmail(VerifyCodeDTO verifyCode)//
        {
            if(ModelState.IsValid)
            {
                var result =await _authService.ConfirmEmailAsync(verifyCode);
                if (result.Id == 1)
                {
                    return Ok("the email confirmed.");
                }
                return BadRequest(result.Message);
            }
            return BadRequest(ModelState);
        }
        [HttpPost("ResendEmailConfirmationCode/{email}")]
        public async Task<ActionResult>ResendEmailConfirmationCode(string email)//
        {
            var result=await _authService.ResendConfirmationCodeAsync(email);
            if(result.Id == 1)
            {
                return Ok("the verification code send.");
            }
            return BadRequest(result.Message);
        }
        [HttpPost("ForgetPassword/{email}")]
        public async Task<ActionResult>ForgetPassword(string email)
        {
            var result = await _authService.ForgetPasswordAsync(email);
            if (result.Id == 1)
            {
                return Ok("verify code sended to change password");
            }
            return BadRequest(result.Message);
        }
        [HttpPost("VerifyCodeForResetPassword")]
        public async Task<ActionResult> VerifyResetPAsswordCodeAsync(VerifyCodeDTO verificationCode)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.VerifyResetCodeAsync(verificationCode);
                if (result.IsAuthenticated)
                {
                    return Ok(result.Token);
                }
                return BadRequest(result.Message);
            }
            return BadRequest(ModelState);
        }
        [HttpPost("ResetPassword")]
        [Authorize]
        public async Task<ActionResult> ResetPassword(ResetPasswordDTO resetPasswordDTO)
        {
            if (ModelState.IsValid)
            {
                var purposeClaim = User.Claims.FirstOrDefault(c => c.Type == "Purpose");

                if (purposeClaim != null && purposeClaim.Value == "ResetPassword")
                {
                    var result = await _authService.ResetPasswordAsync(resetPasswordDTO);
                    if (result.Id == 1)
                    {
                        return Ok("Password is Reset");
                    }
                    return BadRequest(result.Message);
                }
                else
                {
                    return Unauthorized("You are not authorized to reset the password.");
                }
            }
            return BadRequest(ModelState);
        }
        [HttpPost("ResendResetPasswordCodeAsync")]
        public async Task<ActionResult> ResendResetPasswordCodeAsync(string email)
        {
            if(ModelState.IsValid)
            {
                var result = await _authService.ResendResetPasswordCodeAsync(email);
                if(result.Id==1)
                {
                    return Ok("Verificaton code is sent to reset password");
                }
                return BadRequest(result.Message);
            }
            return BadRequest(ModelState);
        }
        void SetRefreshTokenInCookie(string refreshToken, DateTime? expiresIn)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = expiresIn?.ToLocalTime(),
                Secure = true,
                IsEssential = true,
                SameSite = SameSiteMode.None
            };
            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }

    }
}
