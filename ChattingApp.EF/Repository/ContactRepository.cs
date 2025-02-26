using ChattingApp.CORE.DTO.ContactDTO;
using ChattingApp.CORE.DTO.ResultDTO;
using ChattingApp.CORE.Interface;
using CORE.Entities;
using EF.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChattingApp.EF.Repository
{
    public class ContactRepository : IContactRepository
    {
        private readonly AppDbContext _context;
        public ContactRepository(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }
        public IntResult Add(AddContactDTO contact)
        {
            var user1 = _context.Users.Find(contact.UserId);
            var user2=_context.Users.Find(contact.ContactUserId);
            if(user1 is null || user2 is null)
            {
                return new IntResult { Message = "Id is not true" };
            }
            if (!user1.EmailConfirmed)
            {
                return new IntResult { Message = "you should confirm your email to add contact" };
            }
            if (!user2.EmailConfirmed)
            {
                return new IntResult { Message = "this contact should confirm his email to make you add him" };
            }
            var newContact = new Contact(contact);
            _context.Contacts.Add(newContact);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return new IntResult { Message=ex.Message};
            }
            return new IntResult { Id = newContact.Id };
        }
        public IntResult Delete(int id)
        {
            var contact = _context.Contacts.Find(id);
            if (contact is null)
            {
                return new IntResult { Message = "No contact has this Id" };
            }
            _context.Contacts.Remove(contact);
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
        public ShowContactOfUserDTO GetAllContactForUserById(string id)
        {
            var user= _context.Users.Find(id);
            if (user is null)
            {
                return null;
            }
            var contacts = _context.Contacts.Where(x => x.UserId == id)
                .Select(x => new ShowContactInListDTO
                {
                    Id = x.Id,
                    ContactUserName = x.ContactUser.Name,
                    ContactUserEmail = x.ContactUser.Email
                }).ToList();
            var ShowContacts = new ShowContactOfUserDTO(user.Name, user.Email, contacts);
            return ShowContacts;
        }
        public ShowContactDTO GetById(int id)
        {
            var contact = _context.Contacts.Select(x => new ShowContactDTO
            {
                Id = x.Id,
                UserName = x.User.Name,
                Email = x.User.Email,
                ContactUserName = x.ContactUser.Name,
                ContactUserEmail = x.ContactUser.Email
            }).FirstOrDefault(x => x.Id == id);
            return contact;
        }
    }
}
