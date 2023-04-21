using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class UserDto
    {
        [Required]
        public string Forename { get; init; } = null!;

        [Required]
        public string Surname { get; init; } = null!;

        [Required]
        public int? Age { get; init; } = null;
    }
}
