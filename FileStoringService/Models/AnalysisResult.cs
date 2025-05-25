using System;
using System.ComponentModel.DataAnnotations;

namespace FileStoringService.Models
{
    public class AnalysisResult
    {
        [Key]
        public int Id { get; set; }
        public Guid FileId { get; set; }
        public string ResultJson { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}