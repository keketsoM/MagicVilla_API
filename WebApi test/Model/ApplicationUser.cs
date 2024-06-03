using Microsoft.AspNetCore.Identity;

namespace WebApi_test.Model
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
