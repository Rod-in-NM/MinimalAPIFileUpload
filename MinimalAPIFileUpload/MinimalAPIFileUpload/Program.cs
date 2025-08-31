var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
});

// This endpoint is the way Joydig handles file uploads
app.MapPost("/upload", async (IFormFile file) =>
{
    var tempFile = Path.GetTempFileName();
    using var fileStream = File.OpenWrite(tempFile);
    await file.CopyToAsync(fileStream);
});

// This endpoint is the way Joydig handles multiple file uploads
app.MapPost("/upload_multiple_files", async (IFormFileCollection files) =>
{
    foreach (var file in files)
    {
        var tempFile = Path.GetTempFileName();
        using var fileStream = File.OpenWrite(tempFile);
        await file.CopyToAsync(fileStream);
    }
});

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
