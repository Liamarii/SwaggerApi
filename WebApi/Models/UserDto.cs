using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class UserDto
    {
        [Required]
        public string? Forename { get; init; }

        [Required]
        public string? Surname { get; init; }

        [Required]
        public int? Age { get; init; }
    }
}
