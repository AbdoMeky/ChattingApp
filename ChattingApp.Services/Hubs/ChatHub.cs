using ChattingApp.CORE.DTO.ChatMemberDTO;
using ChattingApp.CORE.DTO.MessageDTO;
using ChattingApp.CORE.Interface;
using EF.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ChattingApp.Services.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ICurrentOnlineUserRepository _currentOnlineUserRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IChatMemberRepository _chatMemberRepository;
        private readonly IUserRepository _userRepository;
        public ChatHub(ICurrentOnlineUserRepository currentOnlineUserRepository, IMessageRepository messageRepository)
        {
            _currentOnlineUserRepository = currentOnlineUserRepository;
            _messageRepository = messageRepository;
        }
        public override async Task OnConnectedAsync()
        {
            string connectionId = Context.ConnectionId;
            if (!Context.User.Identity.IsAuthenticated)
            {
                await Clients.Caller.SendAsync("ReceiveError", "User is not authorized.");
                return;
            }
            string userId = Context.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                await Clients.Caller.SendAsync("ReceiveError", "User ID is not valid.");
                return;
            }
            var result = _currentOnlineUserRepository.Add(connectionId, userId);
            if (result.Id == 0)
            {
                await Clients.Caller.SendAsync("ReceiveError", "An error occurred. Please reload the page.");
                return;
            }
            var groups= _userRepository.ChatsIdForUser(userId);
            foreach(var group in groups)
            {
                await Groups.AddToGroupAsync(connectionId,group.ToString());
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            string connectionId = Context.ConnectionId;
            string userId = Context.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                _currentOnlineUserRepository.Delete(connectionId, userId);
                var groups = _userRepository.ChatsIdForUser(userId);
                foreach (var group in groups)
                {
                    await Groups.RemoveFromGroupAsync(connectionId, group.ToString());
                }
            }
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string message,int chatId,IFormFile file,int memberID)
        {
            if (!Context.User.Identity.IsAuthenticated)
            {
                await Clients.Caller.SendAsync("ReceiveError", "User is not authorized.");
                return;
            }
            string userId = Context.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                await Clients.Caller.SendAsync("ReceiveError", "User ID is not valid.");
                return;
            }
            var result=await _messageRepository.Add(new AddMessageDTO
            {
                ChatId = chatId,
                Content = message,
                Media = file,
                MemberId = memberID
            });
            if (result.Id != 0)
            {
                await Clients.Group(chatId.ToString()).SendAsync("ReceiveMessage", _userRepository.UserName(userId), message, file);
            }
            await Clients.Caller.SendAsync("ReceiveError", result.Message);
        }
        /*
        public async Task JoinChat(int chatId)
        {
            if (!Context.User.Identity.IsAuthenticated)
            {
                await Clients.Caller.SendAsync("ReceiveError", "User is not authorized.");
                return;
            }
            string userId = Context.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                await Clients.Caller.SendAsync("ReceiveError", "User ID is not valid.");
                return;
            }
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());

            // إعلام المستخدم بأنه انضم إلى الدردشة
            await Clients.Caller.SendAsync("ReceiveMessage", $"You have joined chat {chatId}.");
        }
        */
        public async Task LeaveChat(int chatId)
        {
            // التحقق مما إذا كان المستخدم مُصادقًا عليه
            if (!Context.User.Identity.IsAuthenticated)
            {
                await Clients.Caller.SendAsync("ReceiveError", "User is not authorized.");
                return;
            }
            string userId = Context.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                await Clients.Caller.SendAsync("ReceiveError", "User ID is not valid.");
                return;
            }
            var result = _chatMemberRepository.Delete(chatId, userId);
            if (string.IsNullOrEmpty(result.Message))
            {
                await Clients.Caller.SendAsync("ReceiveError", result.Message);
            }
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId.ToString());
        }
    }
}
