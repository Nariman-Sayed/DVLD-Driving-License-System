using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace DVLD.Api.IntegrationTests
{
    /// <summary>
    /// Test host that points person image storage at an isolated temporary directory.
    /// </summary>
    public sealed class ImageTestWebApplicationFactory : CustomWebApplicationFactory
    {
        public ImageTestWebApplicationFactory()
        {
            StoragePath = Path.Combine(
                Path.GetTempPath(),
                "dvld-image-tests-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(StoragePath);
        }

        /// <summary>
        /// Gets the temporary directory used as person image storage.
        /// </summary>
        public string StoragePath { get; }

        /// <inheritdoc />
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["Storage:PersonImagesPath"] = StoragePath
                });
            });
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (disposing && Directory.Exists(StoragePath))
            {
                Directory.Delete(StoragePath, recursive: true);
            }

            base.Dispose(disposing);
        }
    }
}
