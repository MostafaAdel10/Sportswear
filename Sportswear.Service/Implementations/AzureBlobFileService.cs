// Sportswear.Service/Implementations/AzureBlobFileService.cs

using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Sportswear.Service.Abstract;

namespace Sportswear.Service.Implementations
{
    public class AzureBlobFileService : IFileService
    {
        private readonly BlobContainerClient _containerClient;
        private readonly long _maxFileSize;
        private readonly HashSet<string> _allowedExtensions;

        public AzureBlobFileService(IOptions<FileUploadOptions> options, IConfiguration configuration)
        {
            var opts = options.Value ?? throw new ArgumentNullException(nameof(options));

            var connectionString = configuration["AzureStorage:ConnectionString"]
                ?? throw new InvalidOperationException("AzureStorage:ConnectionString is missing");

            var containerName = configuration["AzureStorage:ContainerName"] ?? "images";

            _containerClient = new BlobContainerClient(connectionString, containerName);
            _containerClient.CreateIfNotExists(PublicAccessType.Blob);

            _maxFileSize = opts.MaxFileSizeMb * 1024 * 1024;
            _allowedExtensions = new HashSet<string>(
                opts.AllowedExtensions.Select(e => e.ToLowerInvariant()),
                StringComparer.OrdinalIgnoreCase);
        }

        private void ValidateImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ValidationException("File Empty Or Missing");

            if (file.Length > _maxFileSize)
                throw new ValidationException($"File Size Exceeded {_maxFileSize / 1024 / 1024} MB.");

            var ext = Path.GetExtension(file.FileName)?.ToLowerInvariant();
            if (string.IsNullOrEmpty(ext) || !_allowedExtensions.Contains(ext))
                throw new ValidationException("File Type Not Supported. Supported: JPG, PNG, JPEG, WebP.");

            if (!file.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
                throw new ValidationException("File Not Image!");
        }

        private static string GenerateBlobName(IFormFile file, string subFolder)
        {
            var ext = Path.GetExtension(file.FileName)?.ToLowerInvariant() ?? "";
            // بيعمل path زي: products/guid.jpg
            return $"{subFolder.Trim('/')}/{Guid.NewGuid()}{ext}";
        }

        public async Task<string> UploadImageAsync(IFormFile image, string subFolder)
        {
            ValidateImage(image);

            var blobName = GenerateBlobName(image, subFolder);
            var blobClient = _containerClient.GetBlobClient(blobName);

            await blobClient.UploadAsync(image.OpenReadStream(), new BlobHttpHeaders
            {
                ContentType = image.ContentType
            });

            return blobClient.Uri.ToString();
        }

        public async Task<List<string>> UploadImagesAsync(IEnumerable<IFormFile> images, string subFolder)
        {
            if (images == null || !images.Any())
                throw new ValidationException("No Images!");

            var urls = new List<string>();

            foreach (var image in images)
            {
                var url = await UploadImageAsync(image, subFolder);
                urls.Add(url);
            }

            return urls;
        }

        public bool DeleteImage(string? imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl)) return false;

            try
            {
                var uri = new Uri(imageUrl);

                // الـ blob name هو كل حاجة بعد اسم الـ container في الـ URL
                // مثلاً: https://account.blob.core.windows.net/images/products/guid.jpg
                // blobName = products/guid.jpg
                var segments = uri.AbsolutePath.TrimStart('/').Split('/', 2);
                if (segments.Length < 2) return false;

                var blobName = segments[1];
                var blobClient = _containerClient.GetBlobClient(blobName);
                blobClient.DeleteIfExists();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<string> ReplaceImageAsync(string? oldImageUrl, IFormFile newImage, string subFolder)
        {
            var newUrl = await UploadImageAsync(newImage, subFolder);

            if (!string.IsNullOrWhiteSpace(oldImageUrl))
                DeleteImage(oldImageUrl);

            return newUrl;
        }
    }
}