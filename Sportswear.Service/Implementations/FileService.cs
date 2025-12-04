using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Sportswear.Service.Abstract;

namespace Sportswear.Service.Implementations
{
    public class FileService : IFileService
    {
        private readonly string _basePath;
        private readonly string _baseUrl;
        private readonly long _maxFileSize;
        private readonly HashSet<string> _allowedExtensions;

        public FileService(IOptions<FileUploadOptions> options)
        {
            var opts = options.Value ?? throw new ArgumentNullException(nameof(options));

            _baseUrl = opts.BaseUrl.TrimEnd('/');
            _basePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), opts.UploadRoot));
            Directory.CreateDirectory(_basePath);

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
                subFolder = "product-images";

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






        public async Task<string> UploadImageAsync(IFormFile image, string subFolder = "product-images")
        {
            ValidateImage(image);

            var folderPath = GetSafeFolderPath(subFolder);
            var fileName = GenerateFileName(image);
            var filePath = Path.Combine(folderPath, fileName);

            try
            {
                using var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, FileOptions.Asynchronous);
                await image.CopyToAsync(stream);
                await stream.FlushAsync();
            }
            catch (Exception ex)
            {
                if (File.Exists(filePath)) File.Delete(filePath);
                throw new ValidationException("The image failed to be saved on the server.");
            }

            return $"{_baseUrl}/{subFolder}/{fileName}".Replace("//", "/");
        }

        public async Task<List<string>> UploadImagesAsync(IEnumerable<IFormFile> images, string subFolder = "product-images")
        {
            if (images == null || !images.Any())
                throw new ValidationException("No Images!");

            var folderPath = GetSafeFolderPath(subFolder);
            var uploadedUrls = new List<string>();
            var uploadedPhysicalPaths = new List<string>();

            try
            {
                foreach (var image in images)
                {
                    ValidateImage(image);

                    var fileName = GenerateFileName(image);
                    var filePath = Path.Combine(folderPath, fileName);

                    using var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, FileOptions.Asynchronous);
                    await image.CopyToAsync(stream);
                    await stream.FlushAsync();

                    uploadedPhysicalPaths.Add(filePath);
                    uploadedUrls.Add($"{_baseUrl}/{subFolder}/{fileName}".Replace("//", "/"));
                }
            }
            catch (Exception ex)
            {
                foreach (var path in uploadedPhysicalPaths)
                    if (File.Exists(path)) File.Delete(path);

                throw new ValidationException("Failed to upload one or more images.");
            }

            return uploadedUrls;
        }

        public bool DeleteImage(string? imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl)) return false;

            try
            {
                // إزالة الـ BaseUrl (/images) من البداية لو موجودة
                var relativePart = imageUrl.StartsWith(_baseUrl, StringComparison.OrdinalIgnoreCase)
                    ? imageUrl.Substring(_baseUrl.Length).TrimStart('/')
                    : imageUrl.TrimStart('/');

                var fullPath = Path.GetFullPath(Path.Combine(_basePath, relativePart));

                if (!fullPath.StartsWith(_basePath, StringComparison.OrdinalIgnoreCase))
                    return false; // منع traversal

                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new ValidationException($"Failed to delete the image: {imageUrl}");
            }

            return false;
        }

        public async Task<string> ReplaceImageAsync(string? oldImageUrl, IFormFile newImage, string subFolder = "product-images")
        {
            // أول حاجة نرفع الجديد (مع cleanup لو فشل)
            var newUrl = await UploadImageAsync(newImage, subFolder);

            // لو نجح نرفع الجديد → نمسح القديم
            if (!string.IsNullOrWhiteSpace(oldImageUrl))
            {
                DeleteImage(oldImageUrl);
            }

            return newUrl;
        }
    }
}
