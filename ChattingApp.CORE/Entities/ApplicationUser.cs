using ChattingApp.CORE.DTO.AuthDTO;
using ChattingApp.CORE.Entities;
using Microsoft.AspNetCore.Identity;

namespace CORE.Entities
{
    public class ApplicationUser :IdentityUser
    {
        public string Name { get; set; }
        public string? ProfilePictureUrl { get; set; }
        // Navigation Properties
        public ICollection<Contact> ContactsIAdded { get; set; }
        public ICollection<Contact> MyContacts { get; set; }
        public ICollection<ChatMember> ChatMemberships { get; set; }
        public ICollection<RefreshToken>? RefreshTokens { get; set; }
        public ICollection<CurrentOnlineUsers>? CurrentConnectionId { get; set; }
        public ApplicationUser()
        {
            
        }
        public ApplicationUser(RegisterDTO user,string imagePath)
        {
            this.Name = user.Name;
            this.Email = user.Email;
            this.PhoneNumber = user.PhoneNumber;
            this.ProfilePictureUrl = imagePath;
            this.UserName = user.UserName;
            this.EmailConfirmed = false;
            ContactsIAdded = new List<Contact>();
            MyContacts = new List<Contact>();
            ChatMemberships = new List<ChatMember>();
            RefreshTokens = new List<RefreshToken>();
            this.CurrentConnectionId= new List<CurrentOnlineUsers>();
        }
        public void Update(string name,string imagePath)
        {
            this.Name = name;
            this.ProfilePictureUrl= imagePath;
        }
        public void ChangeName(string name)
        {
            this.Name= name;
        }
        public void ChangeProfilePicture(string path)
        {
            this.ProfilePictureUrl = path;
        }
        public ApplicationUser(RegisterDTO user)
        {
            this.Name = user.Name;
            this.Email = user.Email;
            this.PhoneNumber = user.PhoneNumber;
            this.UserName = user.UserName;
            this.EmailConfirmed = false;
            ContactsIAdded = new List<Contact>();
            MyContacts =new List<Contact>();
            ChatMemberships = new List<ChatMember>();
            RefreshTokens =new List<RefreshToken>();
            this.CurrentConnectionId = new List<CurrentOnlineUsers>();
        }
    }
}
