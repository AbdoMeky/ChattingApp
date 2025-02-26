using Azure.Core;
using ChattingApp.CORE.DTO.AuthDTO;
using ChattingApp.CORE.DTO.ResultDTO;
using ChattingApp.CORE.Helper;
using ChattingApp.CORE.Interface;
using ChattingApp.CORE.Services;
using CORE.Entities;
using EF.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
namespace ChattingApp.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JwtSettings _jwtSettings;
        private readonly AppDbContext _context;
        private readonly IUserRepository _userRepository;
        public AuthService(UserManager<ApplicationUser> userManager,
                           RoleManager<IdentityRole> roleManager,
                           IEmailService emailService,
                           SignInManager<ApplicationUser> signInManager,
                           IOptions<JwtSettings> options,
                           AppDbContext context,
                           IUserRepository userRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _emailService = emailService;
            _signInManager = signInManager;
            _jwtSettings = options.Value;
            _context = context;
            _userRepository = userRepository;
        }
        public async Task<AuthResultDTO> LogIn(LogInDTO logInDTO)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(logInDTO.Email);
            if (user is null || !await _userManager.CheckPasswordAsync(user, logInDTO.Password))
            {
                return new AuthResultDTO { Message = "Email or Password is not correct" };
            }
            JwtSecurityToken JwtToken = await CreateToken(user);
            var refreshToken = new RefreshToken();
            if (user.RefreshTokens.Any(t => t.IsActive))
            {
                refreshToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive);
            }
            else
            {
                refreshToken = GenerateRefreshToken();
                user.RefreshTokens.Add(refreshToken);
                try
                {
                    await _userManager.UpdateAsync(user);
                }
                catch (Exception ex)
                {
                    return new AuthResultDTO { Message = ex.Message };
                }
            }
            return new AuthResultDTO
            {
                IsAuthenticated = true,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtToken),
                RefreshToken = refreshToken.Token,
                RefreshTokenExpired = refreshToken.ExpiredOn
            };
        }

        public async Task<AuthResultDTO> RegisteUser(RegisterDTO UserDTO)
        {
            if (await _userManager.FindByEmailAsync(UserDTO.Email) is not null)
            {
                return new AuthResultDTO() { Message = "Email is used." };
            }
            if (await _userManager.FindByNameAsync(UserDTO.UserName) is not null)
            {
                return new AuthResultDTO() { Message = "Username is used." };
            }
            var user = new ApplicationUser(UserDTO);
            var refreshToken = new RefreshToken();
            var oldPath = "";
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var result = await _userManager.CreateAsync(user, UserDTO.Password);
                    if (!result.Succeeded)
                    {
                        var error = "";
                        foreach (var item in result.Errors)
                        {
                            error += " " + item.Description;
                        }
                        return new AuthResultDTO() { Message = error };
                    }
                    var roleResult = await _userManager.AddToRoleAsync(user, "User");
                    if (!roleResult.Succeeded)
                    {
                        var error = "";
                        foreach (var item in roleResult.Errors)
                        {
                            error += " " + item.Description;
                        }
                        return new AuthResultDTO() { Message = error };
                    }
                    refreshToken = GenerateRefreshToken();
                    user.RefreshTokens.Add(refreshToken);
                    var addImageresult = await _userRepository.AddImage(UserDTO.Image, user.Id);
                    oldPath = addImageresult.Id;
                    if (!string.IsNullOrEmpty(addImageresult.Message))
                    {
                        return new AuthResultDTO { Message = addImageresult.Message };
                    }
                    await _userManager.UpdateAsync(user);
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    _userRepository.DeleteImage(oldPath);
                    return new AuthResultDTO() { Message = ex.Message };
                }
            }
            var token = await CreateToken(user);
            var confirmResult =await SendConfirmationEmail(user.Email);
            return new AuthResultDTO()
            {
                IsAuthenticated = true,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken.Token,
                RefreshTokenExpired = refreshToken.ExpiredOn,
                Message= confirmResult.Message
            };
        }
        public async Task<AuthResultDTO> CheckRefreshTokenAndRevoke(string token)
        {
            var user = _userManager.Users.SingleOrDefault(x => x.RefreshTokens.Any(t => t.Token == token));
            if (user == null)
            {
                return new AuthResultDTO() { Message = "not valid token" };
            }
            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);
            if (!refreshToken.IsActive)
            {
                return new AuthResultDTO() { Message = "not active" };
            }
            refreshToken.RevokedOn = DateTime.UtcNow;
            var newRefreshToken = GenerateRefreshToken();
            user.RefreshTokens.Add(newRefreshToken);
            try
            {
                await _userManager.UpdateAsync(user);
            }
            catch (Exception ex)
            {
                return new AuthResultDTO { Message = ex.Message };
            }
            var newToken = await CreateToken(user);
            return new AuthResultDTO()
            {
                IsAuthenticated = true,
                RefreshToken = newRefreshToken.Token,
                RefreshTokenExpired = newRefreshToken.ExpiredOn,
                Token = new JwtSecurityTokenHandler().WriteToken(newToken)
            };
        }
        public async Task<IntResult> ChangePasswordAsync(ChangePasswordDTO newPassword)
        {
            var user = await _userManager.FindByEmailAsync(newPassword.Email);
            if (user == null)
            {
                return new IntResult { Message = "User not found" };
            }
            var oldresult = await _userManager.CheckPasswordAsync(user, newPassword.OldPassword);
            if (!oldresult)
            {
                return new IntResult { Message = "The old password is not correct" };
            }
            var result = await _userManager.ChangePasswordAsync(user, newPassword.OldPassword, newPassword.NewPassword);

            if (result.Succeeded)
            {
                return new IntResult { Id = 1 };
            }
            return new IntResult { Message = result.Errors.Select(e => e.Description).ToList().ToString() };
        }
        public async Task<bool> RevokeRefreshToken(string token)
        {
            var user = _userManager.Users.SingleOrDefault(t => t.RefreshTokens.Any(t => t.Token == token));
            if (user is null)
            {
                return false;
            }
            var refreshToken = user.RefreshTokens.Single(t => t.Token == token);
            if (!refreshToken.IsActive)
            {
                return false;
            }
            refreshToken.RevokedOn = DateTime.UtcNow;
            try
            {
                await _userManager.UpdateAsync(user);
            }
            catch (Exception ex)
            {
                return false;
            }
            await _signInManager.SignOutAsync();
            return true;
        }
        private RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var generator = new RNGCryptoServiceProvider();
            generator.GetBytes(randomNumber);
            return new RefreshToken()
            {
                Token = Convert.ToBase64String(randomNumber),
                ExpiredOn = DateTime.UtcNow.AddDays(1),
                CreatedOn = DateTime.UtcNow
            };
        }
        private async Task<JwtSecurityToken> CreateToken(ApplicationUser user, bool resetPassword = false)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Name,user.Name),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim(JwtRegisteredClaimNames.Jti,new Guid().ToString())
            };
            if (resetPassword == true)
            {
                claims.Add(new Claim("Purpose", "ResetPassword"));
            }
            else
            {
                var Roles = await _userManager.GetRolesAsync(user);
                foreach (var Role in Roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, Role));
                }
            }
            SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken myToken = new JwtSecurityToken(issuer: _jwtSettings.ValidIssuer,
                audience: _jwtSettings.ValidAudiance,
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: credentials);
            return myToken;
        }
        public async Task<IntResult> ConfirmEmailAsync(VerifyCodeDTO request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                return new IntResult { Message = "No user has this email" };
            }
            var storedCodeWithExpiration = await _userManager.GetAuthenticationTokenAsync(user,
                "EmailVerification", "VerificationCode");
            var parts = storedCodeWithExpiration.Split('-');
            if (parts.Length != 2)
            {
                return new IntResult { Message = "VerificationCode is wrong" };
            }

            var storedCode = parts[0];
            var expirationTime = DateTime.Parse(parts[1]);
            if (DateTime.UtcNow > expirationTime)
            {
                throw new Exception("The verification code has expired.");
            }
            if (storedCode != request.VerificationCode)
            {
                return new IntResult { Message = "The verification code is wrong" };
            }
            user.EmailConfirmed = true;
            await _userManager.UpdateAsync(user);
            return new IntResult { Id = 1 };
        }
        async Task<IntResult> SendConfirmationEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
                return new IntResult { Message = "No user has this email" };
            }
            var verificationCode = _emailService.GenerateVerificatonCode();
            var expirationTime = DateTime.UtcNow.AddHours(1);
            await _userManager.SetAuthenticationTokenAsync(user, "EmailVerification", "VerificationCode", $"{verificationCode}-{expirationTime}");

            var emailBody = $"Your verification code is: <b>{verificationCode}</b> and this code will expired at <b>{expirationTime}</b>";

            await _emailService.SendEmailAsync(user.Email, "Verify your email", emailBody);
            return new IntResult { Id = 1 };
        }
        public async Task<IntResult> ForgetPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var verificationCode = _emailService.GenerateVerificatonCode();
                var expirationTime = DateTime.UtcNow.AddHours(1);
                await _userManager.SetAuthenticationTokenAsync(user, "ResetPassword",
                    "ResetPasswordCode", $"{verificationCode}-{expirationTime}");

                await _emailService.SendEmailAsync(email, "Password Reset Verification Code",
                $"Your verification code is: <b>{verificationCode}</b> and this code will expired at <b>{expirationTime}</b>");
                return new IntResult { Id = 1 };
            }
            return new IntResult { Message = "No user has this email" };
        }
        public async Task<AuthResultDTO> VerifyResetCodeAsync(VerifyCodeDTO request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                return new AuthResultDTO { Message = "No user has this email" };
            }
            var storedCodeWithExpiration = await _userManager.GetAuthenticationTokenAsync(user,
                "ResetPassword", "ResetPasswordCode");
            var parts = storedCodeWithExpiration.Split('-');
            if (parts.Length != 2)
            {
                return new AuthResultDTO { Message = "VerificationCode is wrong" };
            }

            var storedCode = parts[0];
            var expirationTime = DateTime.Parse(parts[1]);
            if (DateTime.UtcNow > expirationTime)
            {
                throw new Exception("The verification code has expired.");
            }
            if (storedCode != request.VerificationCode)
            {
                return new AuthResultDTO { Message = "The verification code is wrong" };
            }
            var newToken = await CreateToken(user, true);
            return new AuthResultDTO()
            {
                IsAuthenticated = true,
                Token = new JwtSecurityTokenHandler().WriteToken(newToken)
            };
        }
        public async Task<IntResult> ResetPasswordAsync(ResetPasswordDTO resetPasswordDTO)
        {
            var userModel = await _userManager.FindByEmailAsync(resetPasswordDTO.Email);
            if (userModel == null)
            {
                return new IntResult { Message = "No user has thie email" };
            }
            var removePasswordResult = await _userManager.RemovePasswordAsync(userModel);
            if (!removePasswordResult.Succeeded)
            {
                return new IntResult { Message = "Failed to reset password." };
            }
            var addPasswordResult = await _userManager.AddPasswordAsync(userModel, resetPasswordDTO.NewPassword);
            if (addPasswordResult.Succeeded)
            {
                return new IntResult { Id = 1 };
            }
            return new IntResult { Message = "Invalid password format." };
        }
        public async Task<IntResult> ResendConfirmationCodeAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new IntResult { Message = "No user has this email." };
            }
            if (user.EmailConfirmed)
            {
                return new IntResult { Message = "Email already confirmed." };
            }
            var verificationCode = _emailService.GenerateVerificatonCode();
            var expirationTime = DateTime.UtcNow.AddHours(1);
            await _userManager.SetAuthenticationTokenAsync(user, "EmailVerification",
                "VerificationCode", $"{verificationCode}-{expirationTime}");
            await _emailService.SendEmailAsync(user.Email, "Password Reset Verification Code",
            $"Your verification code is: <b>{verificationCode} </b> and this code will expired at <b>{expirationTime}</b>");
            return new IntResult { Id = 1 };
        }
        public async Task<IntResult> ResendResetPasswordCodeAsync(string Email)
        {
            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
            {
                return new IntResult { Message = "No user has this email." };
            }
            var verificationCode = _emailService.GenerateVerificatonCode();
            var expirationTime = DateTime.UtcNow.AddHours(1);
            await _userManager.SetAuthenticationTokenAsync(user, "ResetPassword", "ResetPasswordCode", $"{verificationCode}-{expirationTime}");
            var emailBody = $"Your password reset verification code is: <b>{verificationCode}</b> </b> and this code will expired at <b>{expirationTime}</b>";
            await _emailService.SendEmailAsync(user.Email, "Password Reset Verification Code", emailBody);
            return new IntResult { Id = 1 };
        }
    }  
}
