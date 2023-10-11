using System.Net.Http.Json;

namespace Demo.Pact.Consumer;

public class ApiClient
{
    private readonly Uri BaseUri;

    public ApiClient(Uri baseUri)
    {
        this.BaseUri = baseUri;
    }

    public async Task<List<WeatherForecast>> GetAllWeatherForecasts()
    {
        using (var client = new HttpClient { BaseAddress = BaseUri })
        {
            var response = await client.GetFromJsonAsync<List<WeatherForecast>>($"/weatherforecast");
            return response!;
        }
    }
    
}