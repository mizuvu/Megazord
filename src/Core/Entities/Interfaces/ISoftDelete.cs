using System;

namespace Zord.Entities.Interfaces
{
    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }

        DateTimeOffset? DeletedOn { get; set; }

        string? DeletedBy { get; set; }
    }
}