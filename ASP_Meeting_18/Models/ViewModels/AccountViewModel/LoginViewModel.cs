using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ASP_Meeting_18.Models.ViewModels.AccountViewModel
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Login")]
        public string Login { get; set; } = default!;
        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = default!;
        [Display(Name ="Remember me")]
        public bool IsPersistent { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
