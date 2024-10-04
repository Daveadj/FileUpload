using FileUpload.Service;
using Microsoft.AspNetCore.Mvc;

namespace FileUpload.Controllers
{
    public class FileUploadController : Controller
    {
        private readonly IFileUploadService _fileUploadService;

        public FileUploadController(IFileUploadService fileUploadService)
        {
            _fileUploadService = fileUploadService;
        }

        [HttpGet]
        public ActionResult UploadFile()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length < 0)
            {
                return BadRequest();
            }

            var savedFileName = await _fileUploadService.SaveFiles(file);

            if (string.IsNullOrEmpty(savedFileName))
            {
                ViewBag.Message = "File upload failed!";
                return View("Index");
            }

            ViewBag.Message = "File uploaded successfully!";
            ViewBag.FileName = savedFileName;
            ViewBag.FilePath = Url.Content($"~/UploadedFiles/{savedFileName}");

            return View();
        }
    }
}