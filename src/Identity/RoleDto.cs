using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Zord.Identity
{
    public class RoleDto
    {
        public string Id { get; set; } = default!;

        [Required]
        [MinLength(3)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public IEnumerable<ClaimDto> Claims { get; set; } = new List<ClaimDto>();
    }
}
