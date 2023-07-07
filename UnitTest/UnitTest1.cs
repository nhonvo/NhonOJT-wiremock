using FluentAssertions;
using System.Net;
using System.Text.Json;
using WebAPI;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace UnitTest;

public class UnitTest1
{
    [Fact]
    public async Task TestGetAsync()
    {
        // Arrange
        var expectedWeatherForecasts = LoadExpectedWeatherForecastsFromJson();

        using var httpClient = new HttpClient();
        using var wireMockServer = CreateWireMockServer(expectedWeatherForecasts);
        var requestUrl = $"{wireMockServer.Urls[0]}/WeatherForecast";

        // Act
        var response = await httpClient.GetAsync(requestUrl);
        response.EnsureSuccessStatusCode(); // Ensure the response is successful

        var jsonContent = await response.Content.ReadAsStringAsync();
        var weatherForecasts = JsonSerializer.Deserialize<List<WeatherForecast>>(jsonContent);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        weatherForecasts.Should().BeEquivalentTo(expectedWeatherForecasts);
    }

    private static List<WeatherForecast> LoadExpectedWeatherForecastsFromJson()
        => JsonSerializer.Deserialize<List<WeatherForecast>>(File.ReadAllText("ExpectedWeatherForecasts.json"))
            ?? throw new ArgumentNullException("Not found the file json");

    private static WireMockServer CreateWireMockServer(List<WeatherForecast> expectedWeatherForecasts)
    {
        var wireMockServer = WireMockServer.Start();
        wireMockServer
            .Given(Request.Create().WithPath("/WeatherForecast"))
            .RespondWith(Response.Create().WithBodyAsJson(expectedWeatherForecasts));
        return wireMockServer;
    }

}