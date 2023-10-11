// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hello, World!");

public record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int) (TemperatureC / 0.5556);
}