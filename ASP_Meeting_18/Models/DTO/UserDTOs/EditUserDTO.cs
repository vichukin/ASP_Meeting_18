using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ASP_Meeting_18.Models.DTO.UserDTOs
{
    public class EditUserDTO
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string Id { get; set; }
        [Required]
        [Display(Name = "Year of birth")]
        public int YearOfBirth { get; set; }
        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; }
    }
}
