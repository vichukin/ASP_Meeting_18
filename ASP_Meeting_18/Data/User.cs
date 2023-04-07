using ASP_Meeting_18.Models.Domain;
using Microsoft.AspNetCore.Identity;

namespace ASP_Meeting_18.Data
{
    public class User : IdentityUser
    {
        public int YearOfBirth { get; set; }
        public List<CartItem>? CartItems { get; set; } =  default;

    }
}
