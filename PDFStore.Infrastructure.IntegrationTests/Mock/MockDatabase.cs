using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace PDFStore.Infrastructure.IntegrationTests.Mock
{
    public class MockDatabase : IDisposable
    {
        private SqliteConnection _connection;

        public DocumentContext Context { get; }

        public MockDatabase()
        {
            _connection = new SqliteConnection("Data Source=:memory:");
            
            _connection.Open();

            var options = new DbContextOptionsBuilder<DocumentContext>()
                .UseSqlite(_connection).Options;

            Context = new DocumentContext(options);

            Context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            _connection.Close();
            Context.Dispose();
        }
    }
}