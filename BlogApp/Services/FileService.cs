using BlogApp.Interfaces;
using Microsoft.AspNetCore.Hosting;

namespace BlogApp.Services
{
    public class FileService(IWebHostEnvironment webHostEnvironment) : IFileService
    {
        public async Task<string> UploadFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new Exception("File is empty");
            }

            const long maxFileSize = 5 * 1024 * 1024;

            if (file.Length > maxFileSize)
            {
                throw new Exception("File size cannot exceed 5 MB");
            }

            string[] allowedExtensions =
            {
                ".jpg",
                ".jpeg",
                ".png"
            };

            var extension = Path.GetExtension(file.FileName).ToLower();

            if (!allowedExtensions.Contains(extension))
            {
                throw new Exception("Invalid file extension");
            }

            string[] allowedMimeTypes =
            {
                "image/jpeg",
                "image/png"
            };

            if (!allowedMimeTypes.Contains(file.ContentType))
            {
                throw new Exception("Invalid file type");
            }

            var fileName = Guid.NewGuid().ToString() + extension;

            var imagesFolderPath = Path.Combine(
                webHostEnvironment.WebRootPath,
                "images"
            );

            if (!Directory.Exists(imagesFolderPath))
            {
                Directory.CreateDirectory(imagesFolderPath);
            }

            var filePath = Path.Combine(imagesFolderPath, fileName);

            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return "/images/" + fileName;
        }

        public void DeleteFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }

            var fullPath = Path.Combine(
                webHostEnvironment.WebRootPath,
                filePath.TrimStart('/')
            );

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
    }
}