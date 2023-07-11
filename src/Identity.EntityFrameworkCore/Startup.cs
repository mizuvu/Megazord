using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zord.Identity.EntityFrameworkCore.Abstractions;
using Zord.Identity.EntityFrameworkCore.Data;
using Zord.Identity.EntityFrameworkCore.Services;

namespace Zord.Identity.EntityFrameworkCore;

public static class Startup
{
    public static IServiceCollection AddIdentityData<TContext>(this IServiceCollection services, IConfiguration config)
        where TContext : IdentityDbContext
    {
        if (config.GetValue<bool>("UseInMemoryDatabase"))
        {
            services
                .AddDbContext<TContext>(options =>
                    options.UseInMemoryDatabase("MemoryDb"));
        }
        else
        {
            var connectionString = config.GetConnectionString("DefaultConnection");

            services.AddDbContext<TContext>(opt =>
                opt.UseSqlServer(connectionString,
                    m => m.MigrationsAssembly("Alpha.Migrator")));
        }

        services.AddScoped<IIdentityDbContext>(provider => provider.GetRequiredService<TContext>());

        return services;
    }

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

    public static IServiceCollection AddClaimProvider<TCustomClaimProvider>(this IServiceCollection services)
        where TCustomClaimProvider : class, IClaimProvider
    {
        services.AddScoped<IClaimProvider, TCustomClaimProvider>();

        return services;
    }

    public static IServiceCollection AddIdentityServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<ITokenService, TokenService>();

        return services;
    }
}