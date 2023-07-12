﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Zord.Identity.Migrator.SeedData;

namespace Zord.Identity.Migrator.MSSQL;

public class AppDbContextInitialiser
{
    private readonly ILogger<AppDbContextInitialiser> _logger;
    private readonly AppIdentityDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public AppDbContextInitialiser(
        ILogger<AppDbContextInitialiser> logger,
        AppIdentityDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task InitialiseAsync()
    {
        _logger.LogInformation("Seeding...");

        try
        {
            if (_context.Database.IsSqlServer() && _context.Database.GetMigrations().Any())
            {
                if ((await _context.Database.GetPendingMigrationsAsync()).Any())
                {
                    await _context.Database.MigrateAsync();

                    var dbName = _context.Database.GetDbConnection().Database;

                    _logger.LogInformation("Database {name} initialized", dbName);
                }
            }
            if (await _context.Database.CanConnectAsync())
            {
                await SeedAsync();
            }

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        // Default roles
        var defaultRole = new ApplicationRole
        {
            Name = DefaultRole.NAME,
            Description = DefaultRole.DESCRIPTION
        };

        if (_roleManager.Roles.All(r => r.Name != defaultRole.Name))
        {
            await _roleManager.CreateAsync(defaultRole);
            _logger.LogInformation("Role {name} added", defaultRole.Name);
        }

        // Default users
        var defaultUser = new ApplicationUser
        {
            UserName = DefaultUser.USER_NAME,
            FirstName = DefaultUser.FIRST_NAME,
            LastName = DefaultUser.LAST_NAME,
            Email = DefaultUser.EMAIL
        };

        if (_userManager.Users.All(u => u.UserName != defaultUser.UserName))
        {
            await _userManager.CreateAsync(defaultUser, DefaultUser.PASSWORD);
            _logger.LogInformation("User {name} added", defaultUser.UserName);

            await _userManager.AddToRolesAsync(defaultUser, new[] { defaultRole.Name });
            _logger.LogInformation("Assigned role {role} to user {user}",
                defaultRole.Name, defaultUser.UserName);
        }

        // Default users
        var basicUser = new ApplicationUser
        {
            UserName = "user",
            FirstName = "Normal",
            LastName = "User",
            Email = "user@domain.local"
        };

        if (_userManager.Users.All(u => u.UserName != basicUser.UserName))
        {
            await _userManager.CreateAsync(basicUser, DefaultUser.PASSWORD);
            _logger.LogInformation("User {name} added", basicUser.UserName);
        }
    }
}
