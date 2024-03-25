global using Zord.Repository;
using Sample.Data;
using Sample.TestOption;
using Zord.Extensions.Caching;
using Zord.Extensions.DependencyInjection;
using Zord.Host;
using Zord.Host.JwtAuth;
using Zord.Host.Middlewares;
using Zord.Host.Swagger;
using Zord.Serilog;
using Zord.SmtpMail;

var builder = WebApplication.CreateBuilder(args);

builder.LoadJsonConfigurations(["configurations"]);
//builder.AddConfigurations(["D:", "configurations"]);
//builder.AddJsonFiles();
builder.Host.ConfigureSerilog();

// Add services to the container.

/*
builder.Services.AddGraphMail(opt =>
{
    opt.ClientSecret = "";
    opt.ClientId = "";
    opt.TenantId = "";
});
*/

builder.Services.AddData(builder.Configuration);
var settings = builder.Configuration.GetSection("Caching").Get<CacheOptions>();
builder.Services.AddCache(opt =>
{
    opt.Provider = settings!.Provider;
    opt.RedisHost = settings.RedisHost;
    opt.RedisPassword = settings.RedisPassword;
});

builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SMTPMail"));

builder.Services.AddFiles();

builder.Services.AddTestOptions(builder.Configuration);

// Overide by BindConfiguration
var issuer = builder.Configuration.GetValue<string>("JWT:Issuer");
var key = builder.Configuration.GetValue<string>("JWT:SecretKey");
builder.Services.AddJwtAuth(issuer!, key!);

//builder.Services.AddTelegram();

builder.Services.AddControllers();

builder.Services.AddApiVersion(1);
builder.Services.AddSwagger(builder.Configuration, false);

builder.Services.AddGlobalExceptionHandler();

builder.Services.AddInboundLogging();

builder.Services.AutoAddDependencies();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(builder.Configuration, false);
}

//app.UseMiddlewares(builder.Configuration);
app.UseInboundLoggingMiddleware(builder.Configuration);
app.UseExceptionHandler();
//app.UseExceptionHandlerMiddleware();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
