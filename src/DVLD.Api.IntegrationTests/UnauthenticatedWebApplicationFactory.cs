using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace DVLD.Api.IntegrationTests
{
    // No authentication overrides here, so the real JWT default scheme from Program.cs
    // applies and requests without a token are rejected with 401 Unauthorized.
    public class UnauthenticatedWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // Inject an obviously-fake JWT key (32+ bytes) so the production fail-fast
            // in AddJwtAuthentication does not break the test host. appsettings.json
            // keeps Jwt:Key empty on purpose.
            builder.UseSetting("Jwt:Key", "integration-test-only-fake-jwt-signing-key-not-for-production-use-0123456789");
            builder.UseSetting("Jwt:Issuer", "DVLD.Api");
            builder.UseSetting("Jwt:Audience", "DVLD.Desktop");

            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["Jwt:Key"] = "integration-test-only-fake-jwt-signing-key-not-for-production-use-0123456789",
                    ["Jwt:Issuer"] = "DVLD.Api",
                    ["Jwt:Audience"] = "DVLD.Desktop"
                });
            });
        }
    }
}
