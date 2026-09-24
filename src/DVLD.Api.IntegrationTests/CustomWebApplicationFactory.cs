using DVLD.Api.Data.Interfaces;
using DVLD.Api.IntegrationTests.Fakes;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace DVLD.Api.IntegrationTests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remove the real IPersonRepository registration so integration tests
                // hit the in-memory fake instead of the actual database.
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IPersonRepository));

                if (descriptor is not null)
                {
                    services.Remove(descriptor);
                }

                services.AddScoped<IPersonRepository, FakePersonRepository>();

                // Register the fixed test identity as the default authentication scheme,
                // overriding the JWT flow from Program.cs so [Authorize] endpoints pass
                // without issuing real tokens.
                services.AddAuthentication(TestAuthenticationHandler.AuthenticationScheme)
                    .AddScheme<AuthenticationSchemeOptions, TestAuthenticationHandler>(
                        TestAuthenticationHandler.AuthenticationScheme,
                        _ => { });
            });
        }
    }
}