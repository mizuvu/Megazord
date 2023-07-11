﻿using System;

namespace Zord.Core.Entities.Interfaces
{
    public interface IAuditableEntity
    {
        DateTimeOffset CreatedOn { get; set; }

        string? CreatedBy { get; set; }

        DateTimeOffset? LastModifiedOn { get; set; }

        string? LastModifiedBy { get; set; }
    }
}