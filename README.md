# WireMock

WireMock is a library and server for stubbing and mocking HTTP-based services. It allows you to create virtual HTTP services that mimic the behavior of real APIs, making it useful for testing and development purposes.

The typical flow to use WireMock for testing an API is as follows:

1. **Create WireMock Server**: Start a WireMock server either programmatically or using Docker.
2. **Define Stubbing**: Set up stubs for the API endpoints that you want to mock. Stubs define the request criteria and the corresponding response behavior.
3. **Make API Requests**: Use an HTTP client (e.g., HttpClient in .NET) to make requests to the WireMock server, simulating requests to the API.
4. **Verify Expectations**: Assert and verify the responses received from the WireMock server against your expectations. This ensures that the API is behaving as expected.

Here's an example of how to use WireMock in a .NET project to test an API:

1. Install WireMock.Net package: Add the WireMock.Net NuGet package to your .NET project.
2. Start WireMock Server: In your test setup or test class constructor, start the WireMock server programmatically or using Docker. For example:

```csharp
using WireMock.Server;

public class YourTestClass
{
    private readonly WireMockServer _wireMockServer;

    public YourTestClass()
    {
        _wireMockServer = WireMockServer.Start();
    }

    // Rest of your test methods

    public void Dispose()
    {
        _wireMockServer.Stop();
    }
}
```

1. Define Stubs: Set up stubs for the API endpoints you want to mock. You can define stubs using fluent syntax or by specifying the mapping and response using JSON files. Here's an example of defining a stub programmatically:

```csharp
_wireMockServer
    .Given(Request.Create().WithPath("/api/endpoint"))
    .Respond(Response.Create().WithStatusCode(200).WithBody("Mocked response"));
```

1. Make API Requests: Use an HTTP client, such as HttpClient, to make requests to the WireMock server, simulating requests to the API endpoint you are testing.

```csharp
using System.Net.Http;
// ...
var httpClient = new HttpClient();
var response = await httpClient.GetAsync($"{_wireMockServer.Urls[0]}/api/endpoint");
var content = await response.Content.ReadAsStringAsync();
```

1. Verify Expectations: Assert and verify the responses received from the WireMock server against your expectations. You can check the status code, response body, headers, etc., to ensure the API is behaving as expected.

```csharp
response.StatusCode.Should().Be(HttpStatusCode.OK);
content.Should().Be("Mocked response");
```

1. Clean Up: Stop the WireMock server and dispose of any resources used in your test class. This ensures proper cleanup after the tests have run.

```csharp
public void Dispose()
{
    _wireMockServer.Stop();
}
```

By following this flow, you can effectively use WireMock to stub and mock API endpoints, allowing you to test your code without relying on real external services.