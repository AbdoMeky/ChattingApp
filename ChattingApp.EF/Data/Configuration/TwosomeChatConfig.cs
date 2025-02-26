using CORE.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Data.Configuration
{
    internal class TwosomeChatConfig : IEntityTypeConfiguration<TwosomeChat>
    {
        public void Configure(EntityTypeBuilder<TwosomeChat> builder)
        {
        }
    }
}
