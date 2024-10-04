
namespace FileUpload.Service
{
    public interface IFileUploadService
    {
        Task<string> SaveFiles(IFormFile file);
    }
}