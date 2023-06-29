using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

using MiniArmory.Data;

namespace MiniArmory.Test
{
    public class InMemoryDbContext
    {
        private readonly SqliteConnection connection;
        private readonly DbContextOptions<MiniArmoryDbContext> contextOptions;

        public InMemoryDbContext()
        {
            connection = new SqliteConnection("Filename=:memory:");
            connection.Open();

            contextOptions = new DbContextOptionsBuilder<MiniArmoryDbContext>()
                .UseSqlite(connection)
                .Options;

            using var context = new MiniArmoryDbContext(contextOptions);

            context.Database.EnsureCreated();
        }

        public MiniArmoryDbContext CreateContext()
            => new MiniArmoryDbContext(contextOptions);

        public void Dispose()
            => connection.Dispose();
    }
}