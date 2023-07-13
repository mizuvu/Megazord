global using Zord.Result;
using Host.Data;
using Host.Identity;
using Host.TestOption;
using Serilog;
using Zord.Extensions.Caching;
using Zord.Extensions.Documents;
using Zord.Extensions.Logging;
using Zord.Extensions.SmtpMail;
using Zord.Extensions.EventBus.RabbitMQ;
using Host.MessageQueues;
using Host.EventHandlers;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(Serilogger.Configure);

// Add services to the container.


builder.Services.AddRabbitMQ(opt =>
{
    opt.HostName = "localhost";
    opt.UserName = "guest";
    opt.Password = "guest";
});
//builder.Services.Subscribe<MessageQueue<TestModel>, TestEventHandler>();
builder.Services.SubscribeMessage<TestMessageQueue, TestMessageQueueEventHandler>();

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

builder.Services.AddAuth(builder.Configuration);

builder.Services.AddTestOptions(builder.Configuration);

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
