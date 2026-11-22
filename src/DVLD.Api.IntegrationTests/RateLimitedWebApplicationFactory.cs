using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace DVLD.Api.IntegrationTests
{
    public class RateLimitedWebApplicationFactory : CustomWebApplicationFactory
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            // Added after the base host so this source wins for the login policy.
            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["RateLimiting:LoginPermitLimit"] = "2",
                    ["RateLimiting:LoginWindowSeconds"] = "60"
                });
            });
        }
    }
}
