using ChattingApp.CORE.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChattingApp.EF.Data.Configuration
{
    public class MessageStatusForChatMemberConfig : IEntityTypeConfiguration<MessageStatusForChatMember>
    {
        public void Configure(EntityTypeBuilder<MessageStatusForChatMember> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.ChatMember).WithMany(x => x.messageStatusForChatMembers).HasForeignKey(x => x.MemberId);
            builder.HasOne(x => x.Message).WithMany(x => x.MessageStatusForChatMembers).HasForeignKey(x => x.MessageId);
            builder.ToTable("MessageStatusForChatMember");
        }
    }
}
