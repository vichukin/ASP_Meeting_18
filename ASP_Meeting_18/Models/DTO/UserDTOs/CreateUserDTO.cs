using System.ComponentModel.DataAnnotations;

namespace ASP_Meeting_18.Models.DTO.UserDTOs
{
    public class CreateUserDTO
    {
        [Required]
        public string Login { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [Display(Name = "Year of birth")]
        public int YearOfBirth { get; set; }
        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; }

    }
}
