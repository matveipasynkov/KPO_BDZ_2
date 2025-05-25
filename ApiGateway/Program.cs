var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

// Configure services URLs
var fileStoringServiceUrl = builder.Configuration["Services:FileStoringService"] ?? "http://filestoringservice:5001";
var fileAnalysisServiceUrl = builder.Configuration["Services:FileAnalysisService"] ?? "http://fileanalysisservice:5002";

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

// Configure routing
app.MapPost("/api/files/upload", async (IFormFile file, IHttpClientFactory clientFactory) =>
{
    if (file == null || file.Length == 0)
        return Results.BadRequest("No file uploaded");

    var client = clientFactory.CreateClient();
    var content = new MultipartFormDataContent();
    content.Add(new StreamContent(file.OpenReadStream()), "file", file.FileName);
    
    var response = await client.PostAsync($"{fileStoringServiceUrl}/api/files", content);
    var result = await response.Content.ReadAsStringAsync();
    
    if (!response.IsSuccessStatusCode)
        return Results.StatusCode((int)response.StatusCode);
        
    return Results.Ok(result);
})
.DisableAntiforgery();

app.MapGet("/api/files", async (IHttpClientFactory clientFactory) =>
{
    var client = clientFactory.CreateClient();
    var response = await client.GetAsync($"{fileStoringServiceUrl}/api/files");
    return await response.Content.ReadAsStringAsync();
});

app.MapGet("/api/analysis", async (IHttpClientFactory clientFactory) =>
{
    var client = clientFactory.CreateClient();
    var response = await client.GetAsync($"{fileAnalysisServiceUrl}/api/analysis");
    return await response.Content.ReadAsStringAsync();
});

app.MapDelete("/api/files/{id}", async (string id, IHttpClientFactory clientFactory) =>
{
    var client = clientFactory.CreateClient();
    var response = await client.DeleteAsync($"{fileStoringServiceUrl}/api/files/{id}");
    if (response.IsSuccessStatusCode)
        return Results.NoContent();
    return Results.StatusCode((int)response.StatusCode);
});

app.MapControllers();

app.UseHttpsRedirection();

app.Run();
