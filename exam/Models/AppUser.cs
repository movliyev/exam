using Microsoft.AspNetCore.Identity;

namespace exam.Models
{
    public class AppUser:IdentityUser
    {
        public string  Name { get; set; }
        public string  Surname { get; set; }
    }
}
