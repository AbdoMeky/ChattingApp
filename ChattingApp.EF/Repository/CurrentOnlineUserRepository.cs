using ChattingApp.CORE.DTO.ResultDTO;
using ChattingApp.CORE.Entities;
using ChattingApp.CORE.Interface;
using EF.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChattingApp.EF.Repository
{
    public class CurrentOnlineUserRepository : ICurrentOnlineUserRepository
    {
        private readonly AppDbContext _context;
        public CurrentOnlineUserRepository(AppDbContext context)
        {
            _context = context;
        }
        public IntResult Add(string connectionId, string userId)
        {
            var currentUser=new CurrentOnlineUsers { ConnectionId=connectionId,UserID=userId};
            _context.CurrentOnlineUsers.Add(currentUser);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return new IntResult { Message = ex.Message };
            }
            return new IntResult { Id = 1 };
        }

        public IntResult Delete(string connectionId, string userId)
        {
            var currentUser = _context.CurrentOnlineUsers.FirstOrDefault(x => x.ConnectionId == connectionId && x.UserID == userId);
            if (currentUser is null)
            {
                return new IntResult { Message = "No User Matching this" };
            }
            _context.CurrentOnlineUsers.Remove(currentUser);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return new IntResult { Message = ex.Message };
            }
            return new IntResult { Id = 1 };
        }
    }
}
