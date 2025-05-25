using System;
using System.IO;
using System.Threading.Tasks;
using FileStoringService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using FileStoringService.Data;
using Microsoft.Extensions.Configuration;

namespace FileStoringService.Services
{
    public interface IFileService
    {
        Task<FileMetadata> SaveFileAsync(IFormFile file);
        Task<IEnumerable<FileMetadata>> GetAllFilesAsync();
        Task<FileMetadata?> GetFileAsync(Guid id);
        Task<bool> DeleteFileAsync(Guid id);
    }

    public class FileService : IFileService
    {
        private readonly FileDbContext _dbContext;
        private readonly string _uploadPath;

        public FileService(FileDbContext dbContext, IWebHostEnvironment environment, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _uploadPath = configuration["FileStorage:BasePath"] ?? Path.Combine(environment.ContentRootPath, "uploads");
            if (!Directory.Exists(_uploadPath))
            {
                Directory.CreateDirectory(_uploadPath);
            }
        }

        public async Task<FileMetadata> SaveFileAsync(IFormFile file)
        {
            var fileMetadata = new FileMetadata
            {
                Id = Guid.NewGuid(),
                FileName = file.FileName,
                ContentType = file.ContentType,
                Size = file.Length,
                UploadedAt = DateTime.UtcNow
            };

            var filePath = Path.Combine(_uploadPath, fileMetadata.Id.ToString());
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            _dbContext.Files.Add(fileMetadata);
            await _dbContext.SaveChangesAsync();

            return fileMetadata;
        }

        public async Task<IEnumerable<FileMetadata>> GetAllFilesAsync()
        {
            return await _dbContext.Files.ToListAsync();
        }

        public async Task<FileMetadata?> GetFileAsync(Guid id)
        {
            return await _dbContext.Files.FindAsync(id);
        }

        public async Task<bool> DeleteFileAsync(Guid id)
        {
            var file = await _dbContext.Files.FindAsync(id);
            if (file == null)
                return false;

            var filePath = Path.Combine(_uploadPath, file.Id.ToString());
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            _dbContext.Files.Remove(file);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}