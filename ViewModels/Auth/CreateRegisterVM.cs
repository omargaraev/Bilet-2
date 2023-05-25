using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace Bilet_2.ViewModels.Auth
{
    public class CreateRegisterVM
    {
        [Required,MaxLength(100)]
        public string Username { get; set; }
        [Required,MaxLength(255),DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required,MinLength(8),DataType(DataType.Password)]
        public string Password { get; set; }
        [Required, MinLength(8), DataType(DataType.Password),Compare(nameof(Password))]
        public string ConfrimPassword { get; set; }
    }
}
