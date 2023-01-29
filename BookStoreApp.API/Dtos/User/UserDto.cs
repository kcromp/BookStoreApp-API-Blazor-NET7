using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.API.Dtos.User
{
    public class UserDto : LoginUserDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Role { get; set; }
    }
}