using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ASP_Meeting_18.Models.DTO.UserDTOs
{
    public class ChangePasswordDTO
    {
        public string Id { get; set; }
        public string Email { get; set; }
        [Required]
        [Display(Name = "New password")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}
