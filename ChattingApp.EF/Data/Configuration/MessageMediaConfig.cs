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
    public class MessageMediaConfig : IEntityTypeConfiguration<MessageMedia>
    {
        public void Configure(EntityTypeBuilder<MessageMedia> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Message).WithOne(x => x.MessageMedia).HasForeignKey<MessageMedia>(x=>x.MessageId).OnDelete(DeleteBehavior.Cascade);
            builder.ToTable("MessageMedia");
        }
    }
}
