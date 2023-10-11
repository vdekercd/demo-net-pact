using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PactNet;
using PactNet.Matchers;
using Xunit.Abstractions;

namespace Demo.Pact.Consumer.Test;

public class ApiTest
{
    private IPactBuilderV3 _pact;
    private readonly List<WeatherForecast> _weatherForecasts;
    private readonly ApiClient _sut;
    
    public ApiTest(ITestOutputHelper output)
    {
        _weatherForecasts = new List<WeatherForecast>()
        {
            new (new DateTime(2023,10,10), 21, "Mild"),
            new (new DateTime(2023,10,11), 22, "Hot")
        };
        
        var config = new PactConfig
        {
            PactDir = Path.Join("..", "pacts"),
            Outputters = new[] { new XUnitOutput(output) },
            DefaultJsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }
        };

        _pact = PactNet.Pact.V3("ApiClient", "ProductService", config).UsingNativeBackend(9000);
        _sut = new ApiClient(new System.Uri($"http://localhost:9000"));
    }
    
    
    [Fact]
    public async Task GetAllWeatherForecast()
    {
        _pact.UponReceiving("A valid request for all products")
            .Given("products exist")
            .WithRequest(HttpMethod.Get, "/weatherforecast")
            .WillRespond()
            .WithStatus(HttpStatusCode.OK)
            .WithHeader("Content-Type", "application/json; charset=utf-8")
            .WithJsonBody(new TypeMatcher(_weatherForecasts));

        await _pact.VerifyAsync(async ctx => {
           await _sut.GetAllWeatherForecasts();
        });
    }
}