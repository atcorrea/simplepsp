using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Respawn;
using SimplePSP.Infrastructure.Persistence;

namespace SimplePSP.API.IntegrationTests
{
    public class TestBase
    {
        protected HttpClient Client { get; private set; }
        protected IServiceProvider Services { get; private set; }
        protected IConfiguration Configuration { get; private set; }
        protected SimplePSPContext DbContext { get; private set; }

        private readonly TestWebApplication webApplication = new();
        private IServiceScope scope;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Client = webApplication.CreateClient();
            Configuration = webApplication.Services.GetRequiredService<IConfiguration>();
        }

        [SetUp]
        public async Task Setup()
        {
            scope = webApplication.Services.CreateScope();
            Services = scope.ServiceProvider;
            DbContext = Services.GetRequiredService<SimplePSPContext>();

            var connectionString = Configuration.GetConnectionString("SQLServer") ??
                throw new Exception("DB Connection string not found!");

            var respawner = await Respawner.CreateAsync(connectionString, new RespawnerOptions()
            {
                TablesToIgnore =
                [
                    "__EFMigrationsHistory"
                ]
            });
            await respawner.ResetAsync(connectionString);
        }

        [TearDown]
        public void TearDown()
        {
            scope.Dispose();
        }
    }
}