using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace SimplePSP.API.IntegrationTests
{
    public class TestWebApplication : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseConfiguration(new ConfigurationBuilder().AddJsonFile("./appsettings.Integration.json").Build());

            builder.ConfigureServices(services =>
            {
            });

            builder.UseEnvironment("Development");
        }
    }
}