global using Zord.Identity.EntityFrameworkCore;
global using Zord.Identity.EntityFrameworkCore.Models;
global using Zord.Identity.Migrator.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Zord.Identity.Migrator.Extensions;
using Zord.Identity.Migrator.MSSQL;

// set Environment
//Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Live");

using var host = Host.CreateHostBuilder(args).Build();
using var scope = host.Services.CreateScope();
var serviceProvider = scope.ServiceProvider;

var initialiser = serviceProvider.GetRequiredService<AppDbContextInitialiser>();
await initialiser.InitialiseAsync();
await initialiser.SeedAsync();

var _logger = serviceProvider.GetRequiredService<ILogger<AppDbContextInitialiser>>();
_logger.LogInformation("Done.");
Console.ReadKey();
