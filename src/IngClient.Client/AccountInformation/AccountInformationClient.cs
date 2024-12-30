using System.Net.Http.Json;
using IngClient.Client.Models;

internal sealed class AccountInformationClient(IHttpClientFactory httpClientFactory)
{
    public async Task<AccountInfo> AccountDetailsAsync(CancellationToken cancellationToken)
    {
        using var client = httpClientFactory.CreateClient(Constants.IngClientName);
        var accounts = await client.GetFromJsonAsync<AccountInfo[]>("v3/accounts", cancellationToken);

        return accounts.First();
    }

    public async Task<Transactions> TransactionsAsync(AccountInfo account, CancellationToken cancellationToken)
    {
        using var client = httpClientFactory.CreateClient(Constants.IngClientName);
        var transactions = await client.GetFromJsonAsync<Transactions>($"v3/accounts/{account.Id}/transactions", cancellationToken);

        return transactions;
    }
}