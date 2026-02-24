using GameBackend.Infra.Persistence;
using GameBackend.Infra.Persistence.Repositories;

namespace GameBackend.Infra.Tests;

public abstract class BaseIntegrationTest : IClassFixture<DatabaseFixture>
{
    protected readonly DatabaseFixture Fixture;
    protected readonly GameDbContext Context;

    protected readonly UserRepository UserRepository;

    protected BaseIntegrationTest(DatabaseFixture fixture)
    {
        Fixture = fixture;
        Context = Fixture.CreateContext();
        UserRepository = new UserRepository(Context);
    }

    protected string GetShortUnique(string prefix) =>
        $"{prefix}_{Guid.NewGuid().ToString("N")[..12]}";
}
