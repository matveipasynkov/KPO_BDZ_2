using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using FileStoringService.Services;
using FileStoringService.Models;
using System;
using Microsoft.Extensions.Configuration;

namespace FileStoringService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly string _uploadPath;

        public FileController(IFileService fileService, IConfiguration configuration)
        {
            _fileService = fileService;
            _uploadPath = configuration["FileStorage:BasePath"] ?? Path.Combine(Directory.GetCurrentDirectory(), "uploads");
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            var fileMetadata = await _fileService.SaveFileAsync(file);
            return Ok(fileMetadata);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFiles()
        {
            var files = await _fileService.GetAllFilesAsync();
            return Ok(files);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFile(Guid id)
        {
            var file = await _fileService.GetFileAsync(id);
            if (file == null)
                return NotFound();

            var filePath = Path.Combine(_uploadPath, id.ToString());
            if (!System.IO.File.Exists(filePath))
                return NotFound("File not found on disk");

            var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
            return File(fileBytes, file.ContentType, file.FileName);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFile(Guid id)
        {
            var result = await _fileService.DeleteFileAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}