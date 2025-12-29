namespace GameBackend.Worker
{
    public class TestDbConnection(IServiceProvider provider) : BackgroundService
    {
        private readonly IServiceProvider _provider = provider;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _provider.CreateScope();
            try
            {
                var db =
                    scope.ServiceProvider.GetRequiredService<Infra.Persistence.GameDbContext>();
                if (await db.Database.CanConnectAsync(stoppingToken))
                {
                    Console.WriteLine("SUCCESS: Worker successfully connected to Database");
                }
                else
                {
                    Console.WriteLine("ERROR: Could not connect to DB.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CRASH: Dependency Injection failed. Error: {ex.Message}");
            }
        }
    }
}
