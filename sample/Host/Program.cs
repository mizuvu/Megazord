global using Zord.Repository;
global using Zord.Result;
using Host.Data;
using Host.TestOption;
using Zord.Api;
using Zord.Api.JwtAuth;
using Zord.Api.Middlewares;
using Zord.Api.Swagger;
using Zord.Extensions.Caching;
using Zord.Extensions.DependencyInjection;
using Zord.Serilog;
using Zord.SmtpMail;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddSwagger(builder.Configuration, true);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(builder.Configuration, true);
}

app.UseMiddlewares(builder.Configuration);

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
