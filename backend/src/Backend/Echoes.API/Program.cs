using Echoes.Application;
using Echoes.Infrastructure;
using Echoes.Infrastructure.Persistence.Postgresql;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Echoes.API/Program.cs
builder
    .Services.AddDatabaseInfrastructure(builder.Configuration)
    .AddAuthInfrastructure(builder.Configuration)
    .AddRepositoryInfrastructure()
    .AddPolicyAndUtilityServices()
    .AddPubSubRedisInfrastructure(builder.Configuration)
    .AddBufferRedisInfrastructure(builder.Configuration)
    .AddSerializationInfrastructure();

builder.Services.AddApplicationServices();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using var scope = app.Services.CreateScope();

    var db = scope.ServiceProvider.GetRequiredService<GameDbContext>();
    await db.Database.MigrateAsync();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
