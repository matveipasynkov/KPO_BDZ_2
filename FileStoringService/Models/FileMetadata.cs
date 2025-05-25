using System;
using System.ComponentModel.DataAnnotations;

namespace FileStoringService.Models
{
    public class FileMetadata
    {
        [Key]
        public Guid Id { get; set; }
        
        [Required]
        public string FileName { get; set; } = string.Empty;
        
        public string? ContentType { get; set; }
        
        public long Size { get; set; }
        
        public DateTime UploadedAt { get; set; }
    }
}