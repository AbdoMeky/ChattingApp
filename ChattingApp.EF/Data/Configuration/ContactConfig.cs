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
    public class ContactConfig : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.User).WithMany(x => x.MyContacts).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x=>x.ContactUser).WithMany(x=>x.ContactsIAdded).HasForeignKey(x => x.ContactUserId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
