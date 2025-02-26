
using ChattingApp.CORE.Helper;
using ChattingApp.CORE.Interface;
using ChattingApp.CORE.Services;
using ChattingApp.EF.Repository;
using ChattingApp.Services;
using CORE.Entities;
using EF.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Claims;
using System.Text;

namespace ChattingApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
                            b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<AppDbContext>()
                    .AddDefaultTokenProviders();
            builder.Services.AddSignalR();
            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IContactRepository, ContactRepository>();
            builder.Services.AddScoped<IChatMemberRepository, ChatMemberRepository>();
            builder.Services.AddScoped<IGroupChatRepository, GroupChatRepository>();
            builder.Services.AddScoped<ITwosomeChatRepository, TwosomeChatRepository>();
            builder.Services.AddScoped<IMediaMessageRepository, MediaMessageRepository>();
            builder.Services.AddScoped<IMessageRepository, MessageRepository>();
            builder.Services.AddScoped<IMessageStatusForChatMemberRepository,MessageStatusForChatMemberRepository>();
            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JWT"));
            var jwtSettings = builder.Configuration.GetSection("JWT").Get<JwtSettings>();
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings?.ValidIssuer,
                    ValidateAudience = true,
                    ValidAudience = jwtSettings?.ValidAudiance,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtSettings?.SecretKey ?? string.Empty))

                };
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async context =>
                    {
                        var userRepository = context.HttpContext.RequestServices.GetRequiredService<IUserRepository>();
                        var userId = context.Principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                        var user =  userRepository.GetWithRefreshToken(userId);

                        if (user == null || user.RefreshTokens == null || user.RefreshTokens.All(t =>!t.IsActive))
                        {
                            context.Fail("Token has been revoked.");
                        }
                    }
                };
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
