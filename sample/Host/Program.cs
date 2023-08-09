global using Zord.Result;
using Extensions.Telegram;
using Host.Data;
using Host.TestOption;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Serilog;
using System.Configuration;
using Zord.Extensions.Caching;
using Zord.Extensions.DependencyInjection;
using Zord.Extensions.Logging;
using Zord.Extensions.SmtpMail;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(Serilogger.Configure);

// Add services to the container.

/*
builder.Services.AddGraphMail(opt =>
{
    opt.ClientSecret = "";
    opt.ClientId = "";
    opt.TenantId = "";
});
*/

builder.Services.AddZordLogging();

builder.Services.AddData(builder.Configuration);
var settings = builder.Configuration.GetSection("Caching").Get<CacheOptions>();
builder.Services.AddZordCache(opt =>
{
    opt.Provider = settings!.Provider;
    opt.RedisHost = settings.RedisHost;
    opt.RedisPassword = settings.RedisPassword;
});

builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SMTPMail"));

builder.Services.AddZordDocuments();

builder.Services.AddTestOptions(builder.Configuration);

builder.Services.AddTelegram();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
