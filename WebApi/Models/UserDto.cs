using System.ComponentModel;
using WebApi.Models.ValidationAttributes;

namespace WebApi.Models
{
    public class UserDto
    {
        [Required]
        [NotNullOrEmptyOrDefault(ErrorMessage = "Forename is required.")]
        public string Forename { get; init; } = null!;

        [Required]
        [NotNullOrEmptyOrDefault(ErrorMessage = "Surname is required.")]
        public string Surname { get; init; } = null!;

        [Required]
        [Range(10, 100)]
        [DefaultValue(0)]
        public int Age { get; init; }
    }
}
