using System.ComponentModel.DataAnnotations;

namespace Bilet_2.Areas.Admin.ViewModels
{
    public class UpdateTeamVM
    {
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; }
        [Required, MaxLength(100)]
        public string Possition { get; set; }
        [Required]
        public IFormFile Photo { get; set; }

    }
}
