using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Sportswear.Service.Abstract;

namespace Sportswear.Service.Implementations
{
    public class FileService : IFileService
    {
        private readonly string _basePath;
        //private readonly string _baseUrl;
        private readonly long _maxFileSize;
        private readonly HashSet<string> _allowedExtensions;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FileService(IOptions<FileUploadOptions> options, IHttpContextAccessor httpContextAccessor)
        {
            var opts = options.Value ?? throw new ArgumentNullException(nameof(options));

            _httpContextAccessor = httpContextAccessor;

            //_baseUrl = opts.BaseUrl.TrimEnd('/');

            _basePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), opts.UploadRoot));
            Directory.CreateDirectory(_basePath);

            _maxFileSize = opts.MaxFileSizeMb * 1024 * 1024;
            _allowedExtensions = new HashSet<string>(
                opts.AllowedExtensions.Select(e => e.ToLowerInvariant()),
                StringComparer.OrdinalIgnoreCase);
        }

        private string GetBaseUrl()
        {
            var request = _httpContextAccessor.HttpContext?.Request;
            return $"{request?.Scheme}://{request?.Host}";
        }

        private void ValidateImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ValidationException("File Empty Or Missing");

            if (file.Length > _maxFileSize)
                throw new ValidationException($"File Size Exceeded {_maxFileSize / 1024 / 1024} MB).");

            var ext = Path.GetExtension(file.FileName)?.ToLowerInvariant();
            if (string.IsNullOrEmpty(ext) || !_allowedExtensions.Contains(ext))
                throw new ValidationException("File Type Not Supported. Supported: JPG, PNG, GIF, WebP.");

            if (!file.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
                throw new ValidationException("File Not Image!");
        }

        private string GetSafeFolderPath(string subFolder)
        {
            if (string.IsNullOrWhiteSpace(subFolder))
                throw new ValidationException("Sub Folder Is Empty!");

            // منع Path Traversal 100% + normalization
            var normalized = Path.Combine(_basePath, subFolder.Replace("..", "").TrimStart('/'));
            var fullPath = Path.GetFullPath(normalized);

            if (!fullPath.StartsWith(_basePath, StringComparison.OrdinalIgnoreCase))
                throw new ValidationException("Invalid Folder Path.");

            Directory.CreateDirectory(fullPath);
            return fullPath;
        }

        private string GenerateFileName(IFormFile file)
            => $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)?.ToLowerInvariant() ?? ""}";






        public async Task<string> UploadImageAsync(IFormFile image, string subFolder)
        {
            ValidateImage(image);

            var folderPath = GetSafeFolderPath(subFolder);
            var fileName = GenerateFileName(image);
            var filePath = Path.Combine(folderPath, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await image.CopyToAsync(stream);

            var baseUrl = GetBaseUrl();
            return $"{baseUrl}/images/{subFolder}/{fileName}";
        }

        public async Task<List<string>> UploadImagesAsync(IEnumerable<IFormFile> images, string subFolder)
        {
            if (images == null || !images.Any())
                throw new ValidationException("No Images!");

            var folderPath = GetSafeFolderPath(subFolder);
            var uploadedUrls = new List<string>();

            foreach (var image in images)
            {
                ValidateImage(image);

                var fileName = GenerateFileName(image);
                var filePath = Path.Combine(folderPath, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await image.CopyToAsync(stream);

                var baseUrl = GetBaseUrl();
                uploadedUrls.Add($"{baseUrl}/images/{subFolder}/{fileName}");
            }

            return uploadedUrls;
        }

        public bool DeleteImage(string? imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl)) return false;

            try
            {
                var uri = new Uri(imageUrl);
                var relativePart = uri.AbsolutePath.TrimStart('/');

                var fullPath = Path.Combine(_basePath, relativePart);

                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    return true;
                }
            }
            catch
            {
                return false;
            }

            return false;
        }

        public async Task<string> ReplaceImageAsync(string? oldImageUrl, IFormFile newImage, string subFolder)
        {
            var newUrl = await UploadImageAsync(newImage, subFolder);

            if (!string.IsNullOrWhiteSpace(oldImageUrl))
            {
                DeleteImage(oldImageUrl);
            }

            return newUrl;
        }
    }
}
