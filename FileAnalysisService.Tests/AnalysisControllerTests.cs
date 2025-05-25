using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace FileAnalysisService.Tests
{
    public class AnalysisControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public AnalysisControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task AnalyzeFile_ReturnsMockResponse()
        {
            var id = "test123";
            var response = await _client.PostAsync($"/api/analysis/analyze/{id}", null);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var json = await response.Content.ReadAsStringAsync();
            Assert.Contains("Mock: анализ выполнен успешно", json);
        }

        [Fact]
        public async Task CompareFiles_ReturnsMockResponse()
        {
            var id1 = "fileA";
            var id2 = "fileB";
            var response = await _client.GetAsync($"/api/analysis/compare/{id1}/{id2}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var json = await response.Content.ReadAsStringAsync();
            Assert.Contains("Mock: сравнение успешно выполнено", json);
        }
    }
} 