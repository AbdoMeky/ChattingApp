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
    public class GroupChatConfig : IEntityTypeConfiguration<GroupChat>
    {
        public void Configure(EntityTypeBuilder<GroupChat> builder)
        {
            builder.Property(x=>x.GroupName).HasMaxLength(50).IsRequired();
        }
    }
}
