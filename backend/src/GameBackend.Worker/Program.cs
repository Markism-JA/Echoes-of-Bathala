using GameBackend.Infra;
using GameBackend.Worker;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddGameInfrastructure(builder.Configuration);
builder.Services.AddHostedService<Worker>();
builder.Services.AddHostedService<TestDbConnection>();

var host = builder.Build();
host.Run();
