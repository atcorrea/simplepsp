using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Respawn;

namespace SimplePSP.Infrastructure.Persistence.IntegrationTests
{
    public class TestBase
    {
        protected SimplePSPContext Context { get; private set; }

        private Respawner _respawner;
        private readonly string _connectionString;

        public TestBase()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("./appsettings.Integration.json").Build();

            _connectionString = configuration.GetConnectionString("SQLServer") ??
                throw new Exception("Connection string do banco de dados não encontrada!");

            var optionsBuilder = new DbContextOptionsBuilder<SimplePSPContext>();
            optionsBuilder.UseSqlServer(_connectionString);

            Context = new SimplePSPContext(optionsBuilder.Options);
        }

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            await Context.Database.EnsureDeletedAsync();
            await Context.Database.MigrateAsync();

            _respawner = await Respawner.CreateAsync(_connectionString, new RespawnerOptions()
            {
                TablesToIgnore =
                [
                    "__EFMigrationsHistory"
                ]
            });
        }

        [SetUp]
        public async Task Setup()
        {
            await _respawner.ResetAsync(_connectionString);
        }
    }
}