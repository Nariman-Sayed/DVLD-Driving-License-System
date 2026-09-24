using Microsoft.AspNetCore.Mvc.Testing;

namespace DVLD.Api.IntegrationTests
{
    // No authentication overrides here, so the real JWT default scheme from Program.cs
    // applies and requests without a token are rejected with 401 Unauthorized.
    public class UnauthenticatedWebApplicationFactory : WebApplicationFactory<Program>
    {
    }
}