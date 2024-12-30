using System.Net;
using System.Text;

namespace IngClient.Tests.Client;

public class AuthTests
{
    [Test]
    public async Task Auth()
    {
        // Arrange
        using var client = new HttpClient();
        client.BaseAddress = new Uri("https://api.sandbox.ing.com/");

        var clientId = "e77d776b-90af-4684-bebc-521e5b2614dd";

        var parameters = new Dictionary<string, string>
        {
            { "grant_type", "client_credentials" },
            { "client_id", clientId }
        };
        var content = new FormUrlEncodedContent(parameters);

        // Act
        var response = await client.PostAsync("oauth2/token", content);
        var raw = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
}
