using Microsoft.AspNetCore.Identity;
using System.Security.Permissions;

namespace BookStoreApp.API.Data
{
    public class ApiUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
