using Microsoft.EntityFrameworkCore;
using FileStoringService.Models;

namespace FileStoringService.Data
{
    public class FileDbContext : DbContext
    {
        public FileDbContext(DbContextOptions<FileDbContext> options)
            : base(options)
        {
        }

        public DbSet<FileMetadata> Files { get; set; }
        public DbSet<AnalysisResult> AnalysisResults { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FileMetadata>()
                .HasKey(f => f.Id);

            modelBuilder.Entity<FileMetadata>()
                .Property(f => f.FileName)
                .IsRequired();

            modelBuilder.Entity<FileMetadata>()
                .Property(f => f.UploadedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
} 