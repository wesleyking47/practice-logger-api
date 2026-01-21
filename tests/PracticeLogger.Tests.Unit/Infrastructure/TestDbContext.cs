using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using PracticeLogger.Infrastructure.Persistence;

namespace PracticeLogger.Tests.Unit.Infrastructure;

public sealed class TestDbContext : IDisposable
{
    private readonly SqliteConnection _connection;

    public TestDbContext()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(_connection)
            .Options;

        Context = new ApplicationDbContext(options);
        Context.Database.EnsureCreated();
    }

    public ApplicationDbContext Context { get; }

    public void Dispose()
    {
        Context.Dispose();
        _connection.Dispose();
    }
}
