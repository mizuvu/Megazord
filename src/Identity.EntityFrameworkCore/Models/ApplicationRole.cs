﻿using Microsoft.AspNetCore.Identity;
using Zord.Core.Entities.Interfaces;

namespace Zord.Identity.EntityFrameworkCore.Models;

public class ApplicationRole : IdentityRole, IEntity, IAuditableEntity
{
    public string? Description { get; set; }

    public DateTimeOffset CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTimeOffset? LastModifiedOn { get; set; }

    public string? LastModifiedBy { get; set; }
}