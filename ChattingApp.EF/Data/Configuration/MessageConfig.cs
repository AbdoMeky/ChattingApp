using CORE.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace EF.Data.Configuration
{
    public class MessageConfig : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x=>x.Member).WithMany(x=>x.Messages).HasForeignKey(x=>x.MemberId).OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(x => x.Chat).WithMany(x => x.Messages).HasForeignKey(x => x.ChatId).OnDelete(DeleteBehavior.SetNull);
            builder.Property(x=>x.Status).HasColumnType("tinyint");
            builder.Property(x=>x.Content).HasMaxLength(1024);
            builder.ToTable("Messages");
        }
    }
}
