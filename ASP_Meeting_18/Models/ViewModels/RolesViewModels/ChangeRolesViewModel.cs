using Microsoft.AspNetCore.Identity;

namespace ASP_Meeting_18.Models.ViewModels.RolesViewModels
{
    public class ChangeRolesViewModel
    {   
         public string UserId { get; set; }
        public string Username { get; set; }
        public IList<string> UserRoles { get; set; }
        public IList<IdentityRole> AllRoles { get; set; }
    }
}
