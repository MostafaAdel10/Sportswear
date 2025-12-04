namespace Sportswear.Service.Implementations
{
    public class FileUploadOptions
    {
        public string UploadRoot { get; set; } = "wwwroot/images"; // المسار النسبي من Directory.GetCurrentDirectory()
        public string BaseUrl { get; set; } = "/images";           // الـ URL اللي هترجع للـ frontend
        public long MaxFileSizeMb { get; set; } = 10;
        public string[] AllowedExtensions { get; set; } =
            { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
    }
}
