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

// This endpoint is the way Joydig wants to handle file uploads
app.MapPost("/upload", async (IFormFile file) =>
{
    if (file.Length > 0)
    {
        var filePath = Path.Combine(Path.GetTempPath(), file.FileName);
        using (var stream = System.IO.File.Create(filePath))
        {
            await file.CopyToAsync(stream);
        }
        return Results.Ok(new { file.FileName, file.Length, filePath });
    }
    return Results.BadRequest("No file uploaded.");
});

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
