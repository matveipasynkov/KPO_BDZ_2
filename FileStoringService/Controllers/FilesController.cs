using Microsoft.AspNetCore.Mvc;
using FileStoringService.Services;
using FileStoringService.Models;

namespace FileStoringService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilesController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FilesController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            Console.WriteLine($"[LOG] UploadFile called. file is null: {file == null}, file name: {file?.FileName}, file length: {file?.Length}");
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            var fileMetadata = await _fileService.SaveFileAsync(file);
            return Ok(fileMetadata);
        }

        [HttpGet]
        public async Task<IActionResult> GetFiles()
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

            return Ok(file);
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