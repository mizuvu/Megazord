global using Zord.Result;
using Extensions.Telegram;
using Host.Data;
using Host.TestOption;
using Serilog;
using Zord.Extensions.Caching;
using Zord.Extensions.Documents;
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

builder.Services.AddZordMemoryCache();

builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SMTPMail"));

builder.Services.AddZordDocumentGenerator();

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
