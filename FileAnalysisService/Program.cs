public partial class Program {
    public static void Main(string[] args) => CreateApp(args).Run();

    public static WebApplication CreateApp(string[] args) {
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();

        app.MapGet("/api/analysis/compare/{fileId1}/{fileId2}", (string fileId1, string fileId2) =>
            Results.Ok(new { FileId1 = fileId1, FileId2 = fileId2, Result = "Mock: сравнение успешно выполнено" })
        );

        app.MapPost("/api/analysis/analyze/{fileId}", (string fileId) =>
            Results.Ok(new { FileId = fileId, Status = "Mock: анализ выполнен успешно" })
        );

        app.Run("http://0.0.0.0:6002");
        return app;
    }
}
