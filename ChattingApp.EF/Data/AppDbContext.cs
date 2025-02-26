using ChattingApp.CORE.Entities;
using CORE.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace EF.Data
{
    public class AppDbContext: IdentityDbContext<ApplicationUser>
    {
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Contact>Contacts { get; set; }
        public DbSet<GroupChat> GroupChats { get; set; }
        public DbSet<ChatMember> ChatMembers { get; set; }
        public DbSet<MessageMedia> MessageMedias { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<TwosomeChat> TwosomeChats { get; set; }
        public DbSet<MessageStatusForChatMember> MessageStatusForChatMember {  get; set; }
        public DbSet<CurrentOnlineUsers> CurrentOnlineUsers { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }

    }
}
