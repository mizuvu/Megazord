using System.ComponentModel.DataAnnotations;

namespace Zord.Identity
{
    public class CreateRoleRequest
    {
        [Required]
        [MinLength(3)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}
