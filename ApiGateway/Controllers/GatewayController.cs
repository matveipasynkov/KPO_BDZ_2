using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace ApiGateway.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GatewayController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public GatewayController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient();
            _configuration = configuration;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Файл не выбран или пустой.");
            try
            {
                var content = new MultipartFormDataContent();
                content.Add(new StreamContent(file.OpenReadStream()), "file", file.FileName);
                var fileAnalysisUrl = _configuration["Services:FileAnalysisService"];
                var response = await _httpClient.PostAsync($"{fileAnalysisUrl}/api/file/upload", content);
                var result = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                    return StatusCode((int)response.StatusCode, result);
                return Content(result, "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка шлюза: {ex.Message}");
            }
        }

        [HttpGet("file/{id}")]
        public async Task<IActionResult> GetFile(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Некорректный идентификатор файла.");
            try
            {
                var fileAnalysisUrl = _configuration["Services:FileAnalysisService"];
                var response = await _httpClient.GetAsync($"{fileAnalysisUrl}/api/file/{id}");
                if (!response.IsSuccessStatusCode)
                    return NotFound();
                var fileBytes = await response.Content.ReadAsByteArrayAsync();
                return File(fileBytes, "application/octet-stream", id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка шлюза: {ex.Message}");
            }
        }

        [HttpPost("analyze/{fileId}")]
        public async Task<IActionResult> AnalyzeFile(string fileId)
        {
            if (string.IsNullOrWhiteSpace(fileId))
                return BadRequest("Некорректный идентификатор файла.");
            try
            {
                var fileStoringUrl = _configuration["Services:FileStoringService"];
                var response = await _httpClient.PostAsync($"{fileStoringUrl}/api/analysis/analyze/{fileId}", null);
                var result = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                    return StatusCode((int)response.StatusCode, result);
                return Content(result, "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка шлюза: {ex.Message}");
            }
        }

        [HttpGet("compare/{fileId1}/{fileId2}")]
        public async Task<IActionResult> CompareFiles(string fileId1, string fileId2)
        {
            if (string.IsNullOrWhiteSpace(fileId1) || string.IsNullOrWhiteSpace(fileId2))
                return BadRequest("Некорректные идентификаторы файлов.");
            try
            {
                var fileStoringUrl = _configuration["Services:FileStoringService"];
                var response = await _httpClient.GetAsync($"{fileStoringUrl}/api/analysis/compare/{fileId1}/{fileId2}");
                var result = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                    return StatusCode((int)response.StatusCode, result);
                return Content(result, "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка шлюза: {ex.Message}");
            }
        }

        [HttpGet("wordcloud/{fileId}")]
        public async Task<IActionResult> GetWordCloud(string fileId)
        {
            if (string.IsNullOrWhiteSpace(fileId))
                return BadRequest("Некорректный идентификатор файла.");
            try
            {
                var fileStoringUrl = _configuration["Services:FileStoringService"];
                var response = await _httpClient.GetAsync($"{fileStoringUrl}/api/analysis/wordcloud/{fileId}");
                var result = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                    return StatusCode((int)response.StatusCode, result);
                return Content(result, "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка шлюза: {ex.Message}");
            }
        }
    }
}