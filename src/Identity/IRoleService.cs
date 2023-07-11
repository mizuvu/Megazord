﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Zord.Result;

namespace Zord.Identity
{
    public interface IRoleService
    {
        /// <summary>
        /// Get all roles
        /// </summary>
        Task<IResult<IEnumerable<RoleDto>>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Get role by ID
        /// </summary>
        Task<IResult<RoleDto>> GetByIdAsync(string id);

        /// <summary>
        /// Get role by Name
        /// </summary>
        Task<IResult<RoleDto>> GetByNameAsync(string id);

        /// <summary>
        /// Create role
        /// </summary>
        Task<IResult<string>> CreateAsync(CreateRoleRequest request);

        /// <summary>
        /// Update role
        /// </summary>
        Task<IResult> UpdateAsync(RoleDto request);

        /// <summary>
        /// Delete role
        /// </summary>
        Task<IResult> DeleteAsync(string id);
    }
}


