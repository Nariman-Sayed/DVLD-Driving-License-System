using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using DVLD.Api.Constants;
using DVLD.Api.IntegrationTests.Fakes;
using DVLD.Shared;
using Xunit;

namespace DVLD.Api.IntegrationTests
{
    public class PersonImageTests : IClassFixture<ImageTestWebApplicationFactory>
    {
        private static readonly byte[] PngBytes =
            [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A, 0x00, 0x01, 0x02];

        private static readonly byte[] JpegBytes =
            [0xFF, 0xD8, 0xFF, 0xE0, 0x00, 0x10, 0x00, 0x01];

        private readonly ImageTestWebApplicationFactory _factory;
        private readonly HttpClient _client;

        public PersonImageTests(ImageTestWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task UploadImage_WithValidPng_StoresGuidFileNameOnly()
        {
            using var form = CreateForm(PngBytes, "../../photo.png");

            var response = await _client.PostAsync("/api/person/2/image", form);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<PersonImageDto>();
            Assert.NotNull(result);
            Assert.EndsWith(".png", result.ImagePath, StringComparison.Ordinal);
            Assert.False(Path.IsPathRooted(result.ImagePath));
            Assert.True(Guid.TryParseExact(
                Path.GetFileNameWithoutExtension(result.ImagePath), "N", out _));

            var savedPath = Path.Combine(_factory.StoragePath, result.ImagePath);
            Assert.True(File.Exists(savedPath));
            Assert.Equal(PngBytes, await File.ReadAllBytesAsync(savedPath));
        }

        [Fact]
        public async Task UploadImage_WithValidJpeg_StoresJpgFileName()
        {
            using var form = CreateForm(JpegBytes, "photo.png");

            var response = await _client.PostAsync("/api/person/2/image", form);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<PersonImageDto>();
            Assert.NotNull(result);
            Assert.EndsWith(".jpg", result.ImagePath, StringComparison.Ordinal);
        }

        [Fact]
        public async Task UploadImage_WithWrongMagicBytes_ReturnsBadRequest()
        {
            using var form = CreateForm([0x01, 0x02, 0x03, 0x04], "photo.png");

            var response = await _client.PostAsync("/api/person/2/image", form);

            await AssertProblemAsync(response, HttpStatusCode.BadRequest, 400, ApiErrorMessages.InvalidImageFile);
        }

        [Fact]
        public async Task UploadImage_WithOversizedFile_ReturnsBadRequest()
        {
            var content = new byte[ImageRules.MaxBytes + 1];
            JpegBytes.CopyTo(content, 0);
            using var form = CreateForm(content, "photo.jpg");

            var response = await _client.PostAsync("/api/person/2/image", form);

            await AssertProblemAsync(response, HttpStatusCode.BadRequest, 400, ApiErrorMessages.InvalidImageFile);
        }

        [Fact]
        public async Task DownloadImage_WithStoredFile_ReturnsImageBytes()
        {
            var path = Path.Combine(_factory.StoragePath, FakePersonRepository.SeededImageFileName);
            await File.WriteAllBytesAsync(path, PngBytes);

            var response = await _client.GetAsync("/api/person/2/image");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("image/png", response.Content.Headers.ContentType?.MediaType);
            Assert.Equal(PngBytes, await response.Content.ReadAsByteArrayAsync());
        }

        [Fact]
        public async Task DownloadImage_WithLegacyAbsolutePath_ReturnsNotFound()
        {
            var response = await _client.GetAsync("/api/person/5/image");

            await AssertProblemAsync(
                response,
                HttpStatusCode.NotFound,
                404,
                ApiErrorMessages.PersonImageNotFound);
        }

        [Fact]
        public async Task DownloadImage_WithNoStoredImage_ReturnsNotFound()
        {
            var response = await _client.GetAsync("/api/person/1/image");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task UploadImage_WithoutStorageConfiguration_ReturnsServerErrorWithSafeDetail()
        {
            using var factory = new CustomWebApplicationFactory();
            using var client = factory.CreateClient();
            using var form = CreateForm(PngBytes, "photo.png");

            var response = await client.PostAsync("/api/person/2/image", form);

            await AssertProblemAsync(
                response,
                HttpStatusCode.InternalServerError,
                500,
                ApiErrorMessages.ImageStorageNotConfigured);
        }

        private static MultipartFormDataContent CreateForm(byte[] bytes, string fileName)
        {
            var form = new MultipartFormDataContent();
            form.Add(new ByteArrayContent(bytes), "file", fileName);
            return form;
        }

        private static async Task AssertProblemAsync(
            HttpResponseMessage response,
            HttpStatusCode expectedStatus,
            int expectedProblemStatus,
            string expectedDetail)
        {
            Assert.Equal(expectedStatus, response.StatusCode);
            Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);

            using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            Assert.Equal(expectedProblemStatus, document.RootElement.GetProperty("status").GetInt32());
            Assert.Equal(expectedDetail, document.RootElement.GetProperty("detail").GetString());
        }
    }
}
