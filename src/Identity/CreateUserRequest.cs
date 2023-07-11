using System.ComponentModel.DataAnnotations;

namespace Zord.Identity
{
    public class CreateUserRequest
    {
        [Required]
        [MinLength(3)]
        public string UserName { get; set; } = default!;

        [Required]
        [MinLength(3)]
        public string FirstName { get; set; } = default!;

        [Required]
        [MinLength(3)]
        public string LastName { get; set; } = default!;

        [Required]
        [MinLength(3)]
        public string Password { get; set; } = default!;

        [EmailAddress]
        public string? Email { get; set; } = default!;

        public string? PhoneNumber { get; set; } = default!;

        public bool UseDomainPassword { get; set; }
    }
}