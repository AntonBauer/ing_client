using System.Net.Http.Json;
using IngClient.Client.AccountInformation;
using IngClient.Client.Authentication;
using IngClient.Client.Models.Account;
using IngClient.Client.Models.Auth;

internal sealed class AccountInformationClient(IHttpClientFactory httpClientFactory)
{
    public async Task<AccountInfo> AccountDetailsAsync(AuthData authData,
                                                       TokenResponse tokenResponse,
                                                       CancellationToken cancellationToken)
    {
        // ToDo: add client dispose
        var client = httpClientFactory.CreateClient(Constants.IngClientName);
        var request = await CreateRequest(authData, tokenResponse, cancellationToken);

        var response = await client.SendAsync(request, cancellationToken);

        var raw = await response.Content.ReadAsStringAsync(cancellationToken);
        var accounts = await response.Content.ReadFromJsonAsync<AccountInfo[]>(cancellationToken);

        return accounts.FirstOrDefault();
    }

    public async Task<Transactions> TransactionsAsync(AccountInfo account, CancellationToken cancellationToken)
    {
        using var client = httpClientFactory.CreateClient(Constants.IngClientName);
        throw new NotImplementedException();
    }

    private static async Task<HttpRequestMessage> CreateRequest(AuthData authData,
                                                                TokenResponse tokenResponse,
                                                                CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, AccountInformationConstants.AccountDetailsEndpoint);
        await AddHeaders(request, authData, tokenResponse, cancellationToken);

        return request;
    }

    private static async Task AddHeaders(HttpRequestMessage request,
                                         AuthData authData,
                                         TokenResponse tokenResponse,
                                         CancellationToken cancellationToken)
    {
        var digest = await request.ComputeDigest(cancellationToken);
        var date = DateTime.UtcNow.ToString("r");
        var signature = request.Sign(date, digest, authData);

        request.Headers.Add("Accept", "application/json");
        request.Headers.Add("X-Request-ID", Guid.NewGuid().ToString());
        request.Headers.Add("Authorization", $"{tokenResponse.TokenType} {tokenResponse.AccessToken}");
        request.Headers.Add("Date", date);
        request.Headers.Add("Digest", digest);
        request.Headers.Add("Signature", signature);
    }
}