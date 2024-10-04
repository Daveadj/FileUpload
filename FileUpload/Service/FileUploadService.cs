using Microsoft.VisualBasic.FileIO;

namespace FileUpload.Service
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly string _savingPath;

        public FileUploadService(IWebHostEnvironment environment)
        {
            _environment = environment;
            _savingPath = Path.Combine(_environment.WebRootPath, "UploadedFiles");
        }

        public async Task<string> SaveFiles(IFormFile file)
        {
            if (file is null || (file != null && file.Length <= 0))
            {
                return string.Empty;
            }
            string path = _savingPath;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string extension = Path.GetExtension(file.FileName);
            string name = $"{Guid.NewGuid().ToString("N")}{extension}";
            string Filename = $"{path}{Path.DirectorySeparatorChar}{name}";
            var rawFile = await ConvertToBase64Async(file);

            await File.WriteAllBytesAsync(Filename, Convert.FromBase64String(rawFile));
            return name;
        }

        private async Task<string> ConvertToBase64Async(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }

            // Read the file content into a byte array
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                var fileBytes = memoryStream.ToArray();

                // Convert the byte array to a Base64 string
                return Convert.ToBase64String(fileBytes);
            }
        }
    }
}