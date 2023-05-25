using Microsoft.CodeAnalysis.Options;
using System.ComponentModel.DataAnnotations;

namespace Bilet_2.Models
{
    public class Team
    {
        public int Id { get; set; }
        [Required,MaxLength(50)]
        public string Name { get; set; }
        [Required, MaxLength(100)]
        public string Possition { get; set; }
        [Required]
        public string ImagePath { get; set; }

    }
}
