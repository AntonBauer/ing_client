using System.Net.Http.Json;

internal sealed class AccountInformationClient(IHttpClientFactory httpClientFactory)
{
    public async Task AccountDetailsAsync(CancellationToken cancellationToken)
    {
        using var client = httpClientFactory.CreateClient(Constants.IngClientName);


        var accounts = await client.GetFromJsonAsync<object>("v3/accounts", cancellationToken);
    }
}