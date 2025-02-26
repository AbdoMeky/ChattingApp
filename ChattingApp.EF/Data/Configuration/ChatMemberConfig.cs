using CORE.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Data.Configuration
{
    internal class ChatMemberConfig : IEntityTypeConfiguration<ChatMember>
    {
        public void Configure(EntityTypeBuilder<ChatMember> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.User).WithMany(x => x.ChatMemberships).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Chat).WithMany(x => x.Members).HasForeignKey(x => x.ChatId).OnDelete(DeleteBehavior.NoAction);
            builder.ToTable("ChatMember");
        }
    }
}
