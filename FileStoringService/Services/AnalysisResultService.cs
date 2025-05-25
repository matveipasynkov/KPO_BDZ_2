using System;
using System.Threading.Tasks;
using FileStoringService.Models;
using Microsoft.EntityFrameworkCore;
using FileStoringService.Data;

namespace FileStoringService.Services
{
    public class AnalysisResultService
    {
        private readonly FileDbContext _context;

        public AnalysisResultService(FileDbContext context)
        {
            _context = context;
        }

        public async Task<AnalysisResult> CreateResultAsync(string resultJson)
        {
            var result = new AnalysisResult
            {
                ResultJson = resultJson,
                CreatedAt = DateTime.UtcNow
            };

            _context.AnalysisResults.Add(result);
            await _context.SaveChangesAsync();

            return result;
        }

        public async Task<AnalysisResult?> GetResultByIdAsync(int id)
        {
            return await _context.AnalysisResults.FindAsync(id);
        }
    }
}