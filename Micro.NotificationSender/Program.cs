using Micro.Domain.Repositories;
using Micro.Domain.ServiceCollectionExtensions;
using Microsoft.Extensions.Hosting;
using BackgroundService = Micro.NotificationSender.Services.BackgroundService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHostedService<BackgroundService>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
