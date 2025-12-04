using Microsoft.AspNetCore.Http;

namespace Sportswear.Service.Abstract
{
    public interface IFileService
    {
        public Task<string> UploadImageAsync(IFormFile image, string subFolder);
        public Task<List<string>> UploadImagesAsync(IEnumerable<IFormFile> images, string subFolder);
        public bool DeleteImage(string? imageUrl);
        public Task<string> ReplaceImageAsync(string? oldImageUrl, IFormFile newImage, string subFolder);
    }
}
