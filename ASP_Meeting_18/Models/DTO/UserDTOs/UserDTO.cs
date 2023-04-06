using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ASP_Meeting_18.Models.DTO.UserDTOs
{
    public class UserDTO
    {
        [Required]
        public string Id { get; set; }
        [Required]

        public string Login { get; set; }
       
        [Required]
        [Display(Name = "Year of birth")]
        public int YearOfBirth { get; set; }
        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; }
    }
}
