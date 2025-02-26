using ChattingApp.CORE.DTO.ContactDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Entities
{
    public class Contact
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string ContactUserId { get; set; }

        // Navigation Properties
        public ApplicationUser User { get; set; }
        public ApplicationUser ContactUser { get; set; }
        public Contact()
        {
            
        }
        public Contact(AddContactDTO contact)
        {
            this.UserId = contact.UserId;
            this.ContactUserId = contact.ContactUserId;
        }
    }

}
