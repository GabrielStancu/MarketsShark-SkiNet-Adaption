using Microsoft.AspNetCore.Identity;

namespace Core.Entities.Identity
{
    //the relevant data about the user to be displayed in the client UI
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }
        public Address Address { get; set; }
    }
}