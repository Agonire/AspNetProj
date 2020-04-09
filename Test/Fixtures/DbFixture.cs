using System;
using DataLayer;
using Microsoft.EntityFrameworkCore;
using WebApp;
using Xunit;

namespace Test.Fixtures
{
    public class DbFixture : IDisposable
    {
        public FilmContext Context { get; }
        public DbContextOptions<FilmContext> Options { get; }
        
        // Runs once per all tests 
        public DbFixture()
        {
            const string connectionString = "server=localhost;database=testwithxunit;uid=Agonire;pwd=1111";
            
            Options = new DbContextOptionsBuilder<FilmContext>()
                .UseMySql(connectionString)
                .Options;
            
            Context = new FilmContext(Options);

            // Context.Database.EnsureDeleted();
            Context.Database.EnsureCreated();
            
            DbTestData.CleanDb(Context);
        }


        public void Dispose()
        {
            // Context.Database.EnsureDeleted();
            Context.Dispose();
        }
    }

    [CollectionDefinition("Database Collection")]
    public class DatabaseCollection : ICollectionFixture<DbFixture>
    {
        // Serves as a container for fixtures
    }
}