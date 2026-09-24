using System.Reflection;
using Xunit;

namespace DVLD.Api.IntegrationTests
{
    public class ArchitectureTests
    {
        [Fact]
        public void Shared_Should_Not_HaveDependencyOn_SqlClient()
        {
            // Arrange
            var sharedAssembly = Assembly.Load("DVLD.Shared");

            // Act
            var referencedAssemblies = sharedAssembly.GetReferencedAssemblies();

            // Assert
            Assert.False(
                referencedAssemblies.Any(a => a.Name != null && a.Name.Contains("SqlClient")),
                "DVLD.Shared must stay pure DTOs and never reference ADO.NET/SqlClient.");
        }
    }
}