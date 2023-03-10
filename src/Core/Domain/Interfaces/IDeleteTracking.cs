using System;

namespace Zord.Core.Domain.Interfaces
{
    public interface IDeleteTracking
    {
        bool IsDeleted { get; set; }
        DateTimeOffset? DeletedOn { get; set; }
        string DeletedBy { get; set; }
    }
}