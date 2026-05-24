namespace BlogApp.Interfaces
{
    public interface IFileService
    {
        Task<string> UploadFileAsync(IFormFile file);

        void DeleteFile(string filePath);
    }
}