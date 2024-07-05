using Microsoft.AspNetCore.Identity;

namespace Library.API.Entities
{
    public class User : IdentityUser
    {
        public DateTimeOffset BirthDate { get; set; }
    }
}
