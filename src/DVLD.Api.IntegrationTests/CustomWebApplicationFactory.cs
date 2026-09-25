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

                // Remove the real IApplicationTypeRepository registration, mirroring the
                // person repository swap above so application type tests use the fake.
                var applicationTypeDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IApplicationTypeRepository));

                if (applicationTypeDescriptor is not null)
                {
                    services.Remove(applicationTypeDescriptor);
                }

                services.AddScoped<IApplicationTypeRepository, FakeApplicationTypeRepository>();

                // Same swap for the test type repository.
                var testTypeDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(ITestTypeRepository));

                if (testTypeDescriptor is not null)
                {
                    services.Remove(testTypeDescriptor);
                }

                services.AddScoped<ITestTypeRepository, FakeTestTypeRepository>();

                // Same swap for the base application repository.
                var applicationDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IBaseApplicationRepository));

                if (applicationDescriptor is not null)
                {
                    services.Remove(applicationDescriptor);
                }

                services.AddScoped<IBaseApplicationRepository, FakeBaseApplicationRepository>();

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