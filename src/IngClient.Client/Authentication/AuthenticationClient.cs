using System.Net.Http.Json;
using IngClient.Client.Authentication.Models;

namespace IngClient.Client.Authentication;

internal sealed class AuthenticationClient(IHttpClientFactory httpClientFactory)
{
    public async Task<TokenResponse> TokenAsync(AuthData authData,
                                                CancellationToken cancellationToken)
    {
        using var client = httpClientFactory.CreateClient(Constants.IngClientName);
        using var request = await CreateRequest(authData);

        var response = await client.SendAsync(request, cancellationToken);

        return await response.Content.ReadFromJsonAsync<TokenResponse>();
    }

    private static async Task<HttpRequestMessage> CreateRequest(AuthData authData)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, AuthConstants.TokenEndpoint)
        {
            Content = new FormUrlEncodedContent(new Dictionary<string, string> { { "grant_type", "client_credentials" } }),
        };

        await AddHeaders(request, authData);

        return request;
    }

    private static async Task AddHeaders(HttpRequestMessage request, AuthData authData)
    {
        var digest = await request.ComputeDigest();
        var date = DateTime.UtcNow.ToString("r");
        var signature = request.Sign(date, digest, authData);

        request.Headers.Add("Accept", "application/json");
        request.Headers.Add("Date", date);
        request.Headers.Add("Digest", digest);
        request.Headers.Add("Authorization", $"Signature keyId=\"{authData.ClientId}\",algorithm=\"rsa-sha256\",headers=\"(request-target) date digest\",signature=\"{signature}\"");
    }
}