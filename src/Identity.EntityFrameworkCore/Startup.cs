using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zord.Identity.EntityFrameworkCore.Abstractions;
using Zord.Identity.EntityFrameworkCore.Data;
using Zord.Identity.EntityFrameworkCore.Services;

namespace Zord.Identity.EntityFrameworkCore;

public static class Startup
{
    /// <summary>
    /// Config Identity data
    /// </summary>
    public static IServiceCollection AddIdentityData<TContext>(this IServiceCollection services, string? connectionString = "", string? migrationsAssembly = "")
        where TContext : IdentityDbContext
    {
        if (string.IsNullOrEmpty(connectionString))
        {
            services.AddDbContext<TContext>(options => 
                options.UseInMemoryDatabase("IdentityDb"));
        }
        else
        {
            if (string.IsNullOrEmpty(migrationsAssembly))
            {
                services.AddDbContext<TContext>(options =>
                    options.UseSqlServer(connectionString));
            }
            else
            {
                services.AddDbContext<TContext>(options =>
                    options.UseSqlServer(connectionString,
                        m => m.MigrationsAssembly(migrationsAssembly)));
            }

        }

        services.AddScoped<IIdentityDbContext>(provider => provider.GetRequiredService<TContext>());

        return services;
    }

    /// <summary>
    /// Config Identity setup
    /// </summary>
    public static IServiceCollection AddIdentitySetup<TContext>(this IServiceCollection services)
        where TContext : IdentityDbContext
    {
        services
            //.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            .AddDefaultIdentity<ApplicationUser>(options =>
            {
                options.SignIn.RequireConfirmedEmail = false;

                // Password settings
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 3;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;

                // Lockout settings
                //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(1);
                //options.Lockout.MaxFailedAccessAttempts = 10;

                // User settings
                options.User.RequireUniqueEmail = true;
            })
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<TContext>()
            .AddDefaultTokenProviders();

        return services;
    }

    private static IServiceCollection AddClaimProvider<TCustomClaimProvider>(this IServiceCollection services)
        where TCustomClaimProvider : class, IClaimType
    {
        services.AddScoped<IClaimType, TCustomClaimProvider>();

        return services;
    }

    /// <summary>
    /// Register Identity services
    /// </summary>
    public static IServiceCollection AddIdentityServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<ITokenService, TokenService>();

        return services;
    }
}